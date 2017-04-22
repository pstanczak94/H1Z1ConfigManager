namespace H1Z1_Config_Manager
{
    partial class ResolutionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResolutionsForm));
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbRefreshRates = new System.Windows.Forms.ComboBox();
            this.listBoxResolutions = new H1Z1_Config_Manager.ListBoxPro();
            this.SuspendLayout();
            // 
            // buttonAccept
            // 
            this.buttonAccept.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAccept.ForeColor = System.Drawing.Color.Green;
            this.buttonAccept.Location = new System.Drawing.Point(27, 269);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(88, 30);
            this.buttonAccept.TabIndex = 3;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCancel.ForeColor = System.Drawing.Color.Brown;
            this.buttonCancel.Location = new System.Drawing.Point(134, 269);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 30);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(39, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Refresh rate:";
            // 
            // cmbRefreshRates
            // 
            this.cmbRefreshRates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRefreshRates.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmbRefreshRates.FormattingEnabled = true;
            this.cmbRefreshRates.ItemHeight = 16;
            this.cmbRefreshRates.Location = new System.Drawing.Point(151, 14);
            this.cmbRefreshRates.MaxDropDownItems = 10;
            this.cmbRefreshRates.MaxLength = 3;
            this.cmbRefreshRates.Name = "cmbRefreshRates";
            this.cmbRefreshRates.Size = new System.Drawing.Size(63, 24);
            this.cmbRefreshRates.TabIndex = 1;
            this.cmbRefreshRates.SelectedIndexChanged += new System.EventHandler(this.comboBoxRefreshRates_SelectedIndexChanged);
            // 
            // listBoxResolutions
            // 
            this.listBoxResolutions.ClickToUnselect = false;
            this.listBoxResolutions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBoxResolutions.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxResolutions.FormattingEnabled = true;
            this.listBoxResolutions.ItemHeight = 20;
            this.listBoxResolutions.Items.AddRange(new object[] {
            "1920x1080",
            "1600x900",
            "1366x768",
            "1280x1024",
            "1280x720",
            "1024x768",
            "800x600"});
            this.listBoxResolutions.Location = new System.Drawing.Point(12, 49);
            this.listBoxResolutions.Name = "listBoxResolutions";
            this.listBoxResolutions.ScrollAlwaysVisible = true;
            this.listBoxResolutions.Size = new System.Drawing.Size(224, 204);
            this.listBoxResolutions.TabIndex = 2;
            this.listBoxResolutions.SelectedIndexChanged += new System.EventHandler(this.listBoxResolutions_SelectedIndexChanged);
            this.listBoxResolutions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxResolutions_MouseDoubleClick);
            // 
            // ResolutionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 313);
            this.Controls.Add(this.cmbRefreshRates);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxResolutions);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAccept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResolutionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "List of available resolutions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private ListBoxPro listBoxResolutions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbRefreshRates;
    }
}