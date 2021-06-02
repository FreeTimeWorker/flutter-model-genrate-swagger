using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flutter_model_genrate_swagger
{
    
    public enum ModelInfoType
    { 
        Obj,
        Enum
    }
    public class ModelInfo
    {
        public ModelInfo() {
            ModelPropties = new List<ModelPropty>();
        }
        public ModelInfoType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ModelPropty> ModelPropties { get; set; }

        public string LowCaseName
        {
            get
            {
                if (string.IsNullOrEmpty(Name) || Name.Length < 2)
                {
                    return Name;
                }
                else
                { 
                    return Name.Substring(0, 1).ToLower() + Name.Substring(1);
                }
            }
        }
        public List<string> Package
        {
            get
            {
                if (this.ModelPropties.Count == 0)
                {
                    return null;
                }
                else
                {
                    var result = new List<string>();
                    foreach (var item in ModelPropties)
                    {
                        if (item.Type == "array")//包含arry
                        {
                            if (string.IsNullOrEmpty(item.SubType))
                            {
                                if (!ServiceAgent.BaseTypes.Contains(item.Type))
                                {
                                    result.Add(item.Type.Substring(0, 1).ToLower() + item.Type.Substring(1));
                                }
                               
                            }
                            else
                            {
                                if (!ServiceAgent.BaseTypes.Contains(item.SubType.ToLower()))
                                {
                                    result.Add(item.SubType.Substring(0, 1).ToLower() + item.SubType.Substring(1));
                                }
                            }
                        }
                        else if (!ServiceAgent.BaseTypes.Contains(item.Type))//基础类型外
                        {
                            result.Add(item.Type.Substring(0, 1).ToLower() + item.Type.Substring(1));
                        }
                    }
                    return result;
                }
            }
        }


    }

    public class ModelPropty
    { 
        public string Name { get; set; }
        /// <summary>
        /// List<....> | 具体类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 如果是arry，这里是arry的子类，
        /// </summary>
        public string SubType { get; set; }
        /// <summary>
        /// 子类描述
        /// </summary>
        public string SubTypeDes { get; set; }
        /// <summary>
        /// 类型描述
        /// </summary>
        public string TypeDes { get; set; }
        public string Format { get; set; }

        public string Description { get; set; }

        public string LowCaseName
        {
            get
            {
                if (string.IsNullOrEmpty(Name)|| Name.Length<2)
                {
                    return Name;
                }
                else
                {
                    return Name.Substring(0, 1).ToLower() + Name.Substring(1);
                }
            }
        }
    }
}
