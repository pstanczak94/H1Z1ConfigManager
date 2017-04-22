namespace H1Z1_Config_Manager
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserOptionsPath = new System.Windows.Forms.TextBox();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnFromRegistry = new System.Windows.Forms.Button();
            this.chooseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserOptions file path";
            // 
            // txtUserOptionsPath
            // 
            this.txtUserOptionsPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserOptionsPath.Location = new System.Drawing.Point(18, 37);
            this.txtUserOptionsPath.MaxLength = 255;
            this.txtUserOptionsPath.Multiline = true;
            this.txtUserOptionsPath.Name = "txtUserOptionsPath";
            this.txtUserOptionsPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUserOptionsPath.Size = new System.Drawing.Size(373, 54);
            this.txtUserOptionsPath.TabIndex = 1;
            this.txtUserOptionsPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUserOptionsPath_KeyPress);
            this.txtUserOptionsPath.Validating += new System.ComponentModel.CancelEventHandler(this.txtUserOptionsPath_Validating);
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(172, 8);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(105, 23);
            this.btnChooseFile.TabIndex = 2;
            this.btnChooseFile.Text = "Choose file";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // btnFromRegistry
            // 
            this.btnFromRegistry.Location = new System.Drawing.Point(283, 8);
            this.btnFromRegistry.Name = "btnFromRegistry";
            this.btnFromRegistry.Size = new System.Drawing.Size(108, 23);
            this.btnFromRegistry.TabIndex = 3;
            this.btnFromRegistry.Text = "From registry";
            this.btnFromRegistry.UseVisualStyleBackColor = true;
            this.btnFromRegistry.Click += new System.EventHandler(this.btnFromRegistry_Click);
            // 
            // chooseFileDialog
            // 
            this.chooseFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.chooseFileDialog_FileOk);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 166);
            this.Controls.Add(this.btnFromRegistry);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.txtUserOptionsPath);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Application settings";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserOptionsPath;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Button btnFromRegistry;
        private System.Windows.Forms.OpenFileDialog chooseFileDialog;
    }
}