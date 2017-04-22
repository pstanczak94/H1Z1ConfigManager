using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class AboutForm : Form
    {
        private MainForm mainForm;

        public AboutForm(MainForm parent)
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            mainForm = parent;

            Text = "About " + Application.ProductName;
            
            Load += AboutForm_Load;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = mainForm.Bounds;

            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }
    }
}
