using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using RazorEngine.Templating;
using RazorEngine;
using RazorEngine.Text;
using RazorEngine.Configuration;

namespace flutter_model_genrate_swagger
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void selectSwaggerjson_Click(object sender, EventArgs e)
        {
            Regex jsonfile = new Regex("^http[s]?://.*\\.json$");
            if (!jsonfile.Match(txtSwaggerjson.Text).Success)
            {
                MessageBox.Show("请输入swagger地址 xxxx.json");
                return;
            }
            Task.Factory.StartNew(() =>
            {
                this.selectSwaggerjson.Invoke(new Action(delegate ()
                {
                    this.selectSwaggerjson.Text = "下载中";
                    this.selectSwaggerjson.Enabled = false;
                }));
                try
                {
                    var httpclient = ServiceAgent.Provider.GetService<IHttpClientFactory>().CreateClient("swaggerdownload");
                    httpclient.GetStreamAsync(txtSwaggerjson.Text)
                    .ContinueWith((s) =>
                    {
                        using (FileStream fs = new FileStream("swagger.json", FileMode.Create))
                        {
                            s.Result.CopyToAsync(fs).Wait();
                        }

                    }).Wait();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("无法从服务端下载swagger.json的情况,可以将 swagger.json文件从浏览器下载后修改文件名为 swagger.json 保存到程序根目录，执行下一步");
                }
                finally
                {
                    this.selectSwaggerjson.Invoke(new Action(delegate ()
                    {
                        this.labmsg.Text = "导入成功";
                        this.selectSwaggerjson.Text = "确定";
                        this.selectSwaggerjson.Enabled = true;
                    }));
                }

            });
        }

        private void btnGenrate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOutput.Text))
            {
                MessageBox.Show("请输入输出目录");
            }

            Task.Factory.StartNew(() =>
            {
                this.selectSwaggerjson.Invoke(new Action(delegate ()
                {
                    this.btnGenrate.Text = "生成中";
                    this.btnGenrate.Enabled = false;
                }));
                try
                {
                    Directory.CreateDirectory(txtOutput.Text);
                    //载入config.
                    var jsonstr = File.ReadAllText("swagger.json");
                    var model = JsonSerializer.Deserialize<SwaggerJsonDoc>(jsonstr);
                    var modelsinfo = GetModesInfo(model);//得到所有的model
                    var config = new TemplateServiceConfiguration();
                    config.Language = Language.CSharp; // VB.NET as template language.
                    config.AllowMissingPropertiesOnDynamic = true;
                    config.CachingProvider = new DefaultCachingProvider(t => { });
                    config.EncodedStringFactory = new HtmlEncodedStringFactory(); // Html encoding.
                    config.Debug = false;
                    var service = RazorEngineService.Create(config);
                    Engine.Razor = service;
                    var objmodel = File.ReadAllText("fluttermodel.cshtml");
                    var enummodel= File.ReadAllText("flutterenum.cshtml");
                    var a = string.Join(',', modelsinfo[0].ModelPropties.Select(o =>
                   {
                       return string.Concat("this.", o.Name);
                   }).ToList());

                    for (int i = 0; i < modelsinfo.Count; i++)
                    {
                        var result = "";
                        if (modelsinfo[i].Type == ModelInfoType.Enum)
                        {
                             result = Engine.Razor.RunCompile(enummodel, "enumModelCreate", null, modelsinfo[i]);
                        }
                        else
                        {
                             result = Engine.Razor.RunCompile(objmodel, "Modelcreate", null, modelsinfo[i]);
                        }
                        if (!string.IsNullOrEmpty(result))
                        {
                            using (var fs = new FileStream(Path.Combine(txtOutput.Text, string.Concat(modelsinfo[i].LowCaseName, ".dart")), FileMode.Create))
                            {
                                using (var sw = new StreamWriter(fs))
                                {
                                    sw.Write(result);
                                }
                            }
                        }
                        this.selectSwaggerjson.Invoke(new Action(delegate ()
                        {
                            this.pb.Maximum = modelsinfo.Count;
                            this.pb.Value = i;
                            this.labmsg.Text = string.Concat("当前进度:", i, "/", modelsinfo.Count);
                        }));
                    }
                    this.selectSwaggerjson.Invoke(new Action(delegate ()
                    {
                        this.labmsg.Text = string.Concat("正在执行代码格式化");
                    }));
                    //执行代码格式化
                    processcmd(txtOutput.Text);
                    this.selectSwaggerjson.Invoke(new Action(delegate ()
                    {
                        this.labmsg.Text = string.Concat("格式化完成,生成成功");
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.selectSwaggerjson.Invoke(new Action(delegate ()
                    {
                        this.btnGenrate.Text = "生成";
                        this.btnGenrate.Enabled = true;
                        this.pb.Maximum = 0;
                        this.pb.Value = 0;
                    }));
                }

            });
        }

        private List<ModelInfo> GetModesInfo(SwaggerJsonDoc model)
        {
            List<ModelInfo> modelInfos = new List<ModelInfo>();
            foreach (var item in model.components.schemas)
            {
                ModelInfo modelinfo = new ModelInfo();
                var ch = model.components.schemas[item.Key];
                modelinfo.Name = item.Key;
                var doc = JsonDocument.Parse(ch.ToString());
               
                if (doc.RootElement.TryGetProperty("description", out JsonElement description))
                { //读取到描述信息
                    modelinfo.Description = description.GetString();
                }
                if (doc.RootElement.TryGetProperty("enum", out JsonElement enumelem))
                {
                    modelinfo.Type = ModelInfoType.Enum;
                    foreach (var e in enumelem.EnumerateArray())
                    {
                        modelinfo.ModelPropties.Add(new ModelPropty()
                        {
                            Name = e.ToString()
                        });
                    }
                }
                else
                {
                    var type = doc.RootElement.GetProperty("type").ToString();
                    if (type == "object")
                    {
                        modelinfo.Type = ModelInfoType.Obj;
                        var properties = doc.RootElement.GetProperty("properties");
                        foreach (var p in properties.EnumerateObject())
                        {
                            ModelPropty modelPropty = new ModelPropty();
                            //p.Name =属性名称
                            modelPropty.Name = p.Name;
                            if (p.Value.ValueKind == JsonValueKind.Object)
                            {
                                if (p.Value.TryGetProperty("type", out JsonElement ptype))
                                {
                                    //获取到类型
                                    modelPropty.Type = ptype.GetString();
                                }
                                else
                                {
                                    //没有类型的情况下找$ref
                                    if (p.Value.TryGetProperty("$ref", out JsonElement oref))
                                    {
                                        modelPropty.Type = oref.GetString()?.Substring(oref.GetString().LastIndexOf('/') + 1);
                                        //尝试判断类型,填充类型描述
                                        if (model.components.schemas.Keys.Contains(modelPropty.Type))
                                        {
                                            var modeldoc = JsonDocument.Parse(model.components.schemas[modelPropty.Type].ToString());
                                            if (modeldoc.RootElement.TryGetProperty("enum", out JsonElement enumprop))
                                            {
                                                modelPropty.TypeDes = "enum";
                                            }
                                            else
                                            {
                                                modelPropty.TypeDes = "object";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        modelPropty.Type = "string";
                                    }
                                }
                                if (p.Value.TryGetProperty("items", out JsonElement arryItemType))
                                {
                                    modelPropty.TypeDes = "arry";
                                    if (arryItemType.TryGetProperty("$ref", out JsonElement subtype))
                                    {
                                        modelPropty.SubType = subtype.GetString()?.Substring(subtype.GetString().LastIndexOf('/') + 1);//list的子类，子类是什么不确定
                                        //尝试判断类型
                                        if (model.components.schemas.Keys.Contains(modelPropty.Type))
                                        {
                                            var modeldoc = JsonDocument.Parse(model.components.schemas[modelPropty.Type].ToString());
                                            if (modeldoc.RootElement.TryGetProperty("enum", out JsonElement enumprop))
                                            {
                                                modelPropty.SubTypeDes = "enum";
                                            }
                                            else
                                            {
                                                modelPropty.SubTypeDes = "object";
                                            }
                                        }
                                    }
                                    else if(p.Value.TryGetProperty("type", out JsonElement arrytypebase))
                                    {
                                        var arrtype = arrytypebase.ValueKind.ToString();//子类
                                        arrtype = arrtype.Replace("string", "String").Replace("integer", "int").Replace("boolean", "bool").Replace("number", "double");
                                        modelPropty.SubType = arrtype;
                                        modelPropty.SubTypeDes = "baseType";

                                    }
                                }
                                if (p.Value.TryGetProperty("description", out JsonElement itemdescription))
                                {
                                    modelPropty.Description = itemdescription.GetString();
                                }
                                if (p.Value.TryGetProperty("format", out JsonElement itemformat))
                                {
                                    modelPropty.Format = itemformat.GetString();
                                }
                                modelinfo.ModelPropties.Add(modelPropty);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("未知类型");
                    }
                }
                modelInfos.Add(modelinfo);
            }
            //using (FileStream fs = new FileStream("log.txt", FileMode.Create))
            //{
            //    using (StreamWriter sw = new StreamWriter(fs))
            //    {
            //        sw.Write(JsonSerializer.Serialize(modelInfos, options: new JsonSerializerOptions()
            //        {
            //            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            //        }));
            //    }
            //}
            return modelInfos;
        }

        private void processcmd(string path)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            p.StandardInput.WriteLine("flutter format "+path);
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine("exit");
            p.WaitForExit();//等待程序执行完退出进程

            p.Close();
        }
    }
}
