namespace H1Z1_Config_Manager
{
    partial class ConsoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.labelLogType = new System.Windows.Forms.Label();
            this.cmbLogType = new System.Windows.Forms.ComboBox();
            this.panelGroupLogType = new System.Windows.Forms.Panel();
            this.panelGroupLogType.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsole.Location = new System.Drawing.Point(0, 43);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsole.Size = new System.Drawing.Size(532, 270);
            this.txtConsole.TabIndex = 2;
            this.txtConsole.WordWrap = false;
            // 
            // labelLogType
            // 
            this.labelLogType.AutoSize = true;
            this.labelLogType.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelLogType.Location = new System.Drawing.Point(14, 14);
            this.labelLogType.Name = "labelLogType";
            this.labelLogType.Size = new System.Drawing.Size(72, 16);
            this.labelLogType.TabIndex = 0;
            this.labelLogType.Text = "Log type:";
            // 
            // cmbLogType
            // 
            this.cmbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmbLogType.FormattingEnabled = true;
            this.cmbLogType.Items.AddRange(new object[] {
            "ALL",
            "DEBUG",
            "INFO",
            "WARNING",
            "ERROR"});
            this.cmbLogType.Location = new System.Drawing.Point(94, 11);
            this.cmbLogType.Name = "cmbLogType";
            this.cmbLogType.Size = new System.Drawing.Size(108, 22);
            this.cmbLogType.TabIndex = 1;
            this.cmbLogType.SelectedIndexChanged += new System.EventHandler(this.cmbLogType_SelectedIndexChanged);
            // 
            // panelGroupLogType
            // 
            this.panelGroupLogType.Controls.Add(this.cmbLogType);
            this.panelGroupLogType.Controls.Add(this.labelLogType);
            this.panelGroupLogType.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGroupLogType.Location = new System.Drawing.Point(0, 0);
            this.panelGroupLogType.Name = "panelGroupLogType";
            this.panelGroupLogType.Padding = new System.Windows.Forms.Padding(10);
            this.panelGroupLogType.Size = new System.Drawing.Size(532, 43);
            this.panelGroupLogType.TabIndex = 4;
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 313);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.panelGroupLogType);
            this.KeyPreview = true;
            this.Name = "ConsoleForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Console";
            this.panelGroupLogType.ResumeLayout(false);
            this.panelGroupLogType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.Label labelLogType;
        private System.Windows.Forms.ComboBox cmbLogType;
        private System.Windows.Forms.Panel panelGroupLogType;
    }
}