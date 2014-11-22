namespace BPExporter
{
    partial class NewProjectDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okBtn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ProjectName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FileNo = new System.Windows.Forms.TextBox();
            this.akzSufix = new System.Windows.Forms.Label();
            this.prefixFileNo = new System.Windows.Forms.TextBox();
            this.sufixFileNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uniqueIP = new System.Windows.Forms.CheckBox();
            this.ProductName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Enabled = false;
            this.okBtn.Location = new System.Drawing.Point(283, 164);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 11;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(364, 164);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Project name:";
            // 
            // ProjectName
            // 
            this.ProjectName.BackColor = System.Drawing.SystemColors.Info;
            this.ProjectName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ProjectName.Location = new System.Drawing.Point(15, 30);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(427, 24);
            this.ProjectName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "AKZ:";
            // 
            // FileNo
            // 
            this.FileNo.BackColor = System.Drawing.SystemColors.Info;
            this.FileNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FileNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FileNo.Location = new System.Drawing.Point(122, 123);
            this.FileNo.Name = "FileNo";
            this.FileNo.Size = new System.Drawing.Size(87, 24);
            this.FileNo.TabIndex = 4;
            this.FileNo.TextChanged += new System.EventHandler(this.FileNo_TextChanged);
            // 
            // akzSufix
            // 
            this.akzSufix.AutoSize = true;
            this.akzSufix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.akzSufix.Location = new System.Drawing.Point(115, 127);
            this.akzSufix.Name = "akzSufix";
            this.akzSufix.Size = new System.Drawing.Size(0, 20);
            this.akzSufix.TabIndex = 6;
            // 
            // prefixFileNo
            // 
            this.prefixFileNo.BackColor = System.Drawing.SystemColors.Info;
            this.prefixFileNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prefixFileNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.prefixFileNo.Location = new System.Drawing.Point(12, 123);
            this.prefixFileNo.Name = "prefixFileNo";
            this.prefixFileNo.Size = new System.Drawing.Size(97, 24);
            this.prefixFileNo.TabIndex = 3;
            // 
            // sufixFileNo
            // 
            this.sufixFileNo.BackColor = System.Drawing.SystemColors.Info;
            this.sufixFileNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sufixFileNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.sufixFileNo.Location = new System.Drawing.Point(234, 123);
            this.sufixFileNo.Name = "sufixFileNo";
            this.sufixFileNo.Size = new System.Drawing.Size(43, 24);
            this.sufixFileNo.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(215, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "/";
            // 
            // uniqueIP
            // 
            this.uniqueIP.AutoSize = true;
            this.uniqueIP.Location = new System.Drawing.Point(364, 128);
            this.uniqueIP.Name = "uniqueIP";
            this.uniqueIP.Size = new System.Drawing.Size(73, 17);
            this.uniqueIP.TabIndex = 10;
            this.uniqueIP.Text = "Unique IP";
            this.uniqueIP.UseVisualStyleBackColor = true;
            this.uniqueIP.CheckedChanged += new System.EventHandler(this.uniqueIP_CheckedChanged);
            // 
            // ProductName
            // 
            this.ProductName.BackColor = System.Drawing.SystemColors.Info;
            this.ProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProductName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ProductName.Location = new System.Drawing.Point(15, 78);
            this.ProductName.Name = "ProductName";
            this.ProductName.Size = new System.Drawing.Size(427, 24);
            this.ProductName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "Product name:";
            // 
            // NewProjectDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 200);
            this.Controls.Add(this.ProductName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.uniqueIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sufixFileNo);
            this.Controls.Add(this.prefixFileNo);
            this.Controls.Add(this.akzSufix);
            this.Controls.Add(this.FileNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProjectName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.okBtn);
            this.Name = "NewProjectDlg";
            this.Text = "NewProjectDlg";
            this.Load += new System.EventHandler(this.NewProjectDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ProjectName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FileNo;
        private System.Windows.Forms.Label akzSufix;
        private System.Windows.Forms.TextBox prefixFileNo;
        private System.Windows.Forms.TextBox sufixFileNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox uniqueIP;
        private System.Windows.Forms.TextBox ProductName;
        private System.Windows.Forms.Label label4;
    }
}