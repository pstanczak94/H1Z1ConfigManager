using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class ConsoleForm : Form
    {
        private MainForm mainForm;

        public ConsoleForm(MainForm parent)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            mainForm = parent;

            Logger.LogFunction = MessageArrived;

            Load += ConsoleForm_Load;
            Shown += ConsoleForm_Shown;

            MinimumSize = Size;

#if !DEBUG
            cmbLogType.Items.Remove("DEBUG");
#endif
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = mainForm.Bounds;

            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }

        private void ConsoleForm_Shown(object sender, EventArgs e)
        {
            cmbLogType.SelectedIndex = -1;
            if (cmbLogType.Items.Count > 0)
                cmbLogType.SelectedIndex = 0;
        }

        public void MessageArrived(LogType type, string message)
        {
            string currentLogType = cmbLogType.Text;

            if (string.Compare(currentLogType, "ALL", true) == 0)
                txtConsole.AppendText(String.Format("[{0}] {1}", type, message));
            else if (string.Compare(currentLogType, type.ToString(), true) == 0)
                txtConsole.AppendText(message);
        }

        private void cmbLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string currentLogType = cmbLogType.Text;
            StringBuilder consoleText = new StringBuilder(5000);

            if (string.Compare(currentLogType, "ALL", true) == 0)
            {
                foreach (LogMessage msg in Logger.Messages)
                    consoleText.Append(String.Format("[{0}] {1}", msg.type, msg.text));
            }
            else
            {
                foreach (LogMessage msg in Logger.Messages)
                    if (string.Compare(currentLogType, msg.type.ToString(), true) == 0)
                        consoleText.Append(msg.text);
            }

            txtConsole.Text = consoleText.ToString();
            txtConsole.Select(txtConsole.Text.Length, 0);
            txtConsole.ScrollToCaret();
            //txtConsole.Focus();
        }
    }
}
