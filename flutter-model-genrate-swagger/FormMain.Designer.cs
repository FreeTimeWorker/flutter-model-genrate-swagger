
namespace flutter_model_genrate_swagger
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.selectSwaggerjson = new System.Windows.Forms.Button();
            this.txtSwaggerjson = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnGenrate = new System.Windows.Forms.Button();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.labmsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectSwaggerjson
            // 
            this.selectSwaggerjson.Location = new System.Drawing.Point(503, 12);
            this.selectSwaggerjson.Name = "selectSwaggerjson";
            this.selectSwaggerjson.Size = new System.Drawing.Size(75, 23);
            this.selectSwaggerjson.TabIndex = 0;
            this.selectSwaggerjson.Text = "确定";
            this.selectSwaggerjson.UseVisualStyleBackColor = true;
            this.selectSwaggerjson.Click += new System.EventHandler(this.selectSwaggerjson_Click);
            // 
            // txtSwaggerjson
            // 
            this.txtSwaggerjson.Location = new System.Drawing.Point(13, 12);
            this.txtSwaggerjson.Name = "txtSwaggerjson";
            this.txtSwaggerjson.PlaceholderText = "http://localhost/swagger/v1/swagger.json";
            this.txtSwaggerjson.Size = new System.Drawing.Size(484, 23);
            this.txtSwaggerjson.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "输出目录";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(70, 52);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(427, 23);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.Text = "D:\\output";
            // 
            // btnGenrate
            // 
            this.btnGenrate.Location = new System.Drawing.Point(503, 52);
            this.btnGenrate.Name = "btnGenrate";
            this.btnGenrate.Size = new System.Drawing.Size(71, 23);
            this.btnGenrate.TabIndex = 4;
            this.btnGenrate.Text = "生成";
            this.btnGenrate.UseVisualStyleBackColor = true;
            this.btnGenrate.Click += new System.EventHandler(this.btnGenrate_Click);
            // 
            // pb
            // 
            this.pb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pb.Location = new System.Drawing.Point(0, 110);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(585, 23);
            this.pb.TabIndex = 5;
            // 
            // labmsg
            // 
            this.labmsg.AutoSize = true;
            this.labmsg.Location = new System.Drawing.Point(13, 90);
            this.labmsg.Name = "labmsg";
            this.labmsg.Size = new System.Drawing.Size(0, 17);
            this.labmsg.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AccessibleDescription = "swagger doc to flutter model";
            this.AccessibleName = "swagger doc to flutter model";
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 133);
            this.Controls.Add(this.labmsg);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.btnGenrate);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSwaggerjson);
            this.Controls.Add(this.selectSwaggerjson);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "auth by zzf";
            this.Text = "swagger doc to flutter model";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button selectSwaggerjson;
        private System.Windows.Forms.TextBox txtSwaggerjson;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnGenrate;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label labmsg;
    }
}

