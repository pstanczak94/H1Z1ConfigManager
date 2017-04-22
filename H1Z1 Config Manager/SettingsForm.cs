using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class SettingsForm : Form
    {
        public const string gameLocationRegKeyName1 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 433850";
        public const string gameLocationRegKeyName2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 433850";
        public const string gameLocationRegValueName = "InstallLocation";

        private MainForm mainForm;

        public SettingsForm(MainForm parent)
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            mainForm = parent;

            btnFromRegistry.Enabled = !string.IsNullOrWhiteSpace(mainForm.pathFromRegistry);
        }

        public static bool IsUserOptionsPathValid(ref string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            path = path.Replace("/", "\\");

            if (path.EndsWith("\\", true, CultureInfo.InvariantCulture))
                path = path.Substring(0, path.Length - 1);

            FileInfo fileInfo = null;

            try
            {
                fileInfo = new FileInfo(path + "\\" + Settings.UserOptionsFileName);
            }
            catch
            {
                Logger.WriteLine(LogType.ERROR, "IsUserOptionsPathValid: Cannot create FileInfo!");
                fileInfo = null;
                return false;
            }

            if (!fileInfo.Exists)
            {
                Logger.WriteLine(LogType.ERROR, "IsUserOptionsPathValid: File doesn't exists!");
                fileInfo = null;
                return false;
            }

            fileInfo = null;
            return true;
        }

        public static string GetRegistryString(string keyName, string valueName)
        {
            RegistryKey localKey;

            if (Environment.Is64BitOperatingSystem)
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            else
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

            RegistryKey subKey = localKey.OpenSubKey(keyName);

            string value = String.Empty;
            
            if (subKey != null)
                value = subKey.GetValue(valueName).ToString();

            return value;
        }

        public static string GetPathFromRegistry()
        {
            string path = GetRegistryString(gameLocationRegKeyName1, gameLocationRegValueName);

            if (string.IsNullOrWhiteSpace(path))
                path = GetRegistryString(gameLocationRegKeyName2, gameLocationRegValueName);
            
            if (!string.IsNullOrWhiteSpace(path))
                if (IsUserOptionsPathValid(ref path))
                    return path;

            return String.Empty;
        }

        public static bool CheckPathIsInRegistry(out string pathRef)
        {
            string path = GetPathFromRegistry();

            if (!string.IsNullOrWhiteSpace(path))
            {
                pathRef = path;
                return true;
            }

            pathRef = String.Empty;
            return false;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = mainForm.Bounds;

            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            txtUserOptionsPath.Text = Settings.UserOptionsPath;
            ActiveControl = null;
        }

        private void txtUserOptionsPath_Validating(object sender, CancelEventArgs e)
        {
            string path = txtUserOptionsPath.Text;
            TryToParsePath(path);
        }

        private delegate bool TryToParsePathDelegate(string path);

        private bool TryToParsePath(string path)
        {
            if (string.Compare(path, Settings.UserOptionsPath, true) == 0)
                return true;

            if (IsUserOptionsPathValid(ref path))
            {
                if (string.Compare(path, Settings.UserOptionsPath, true) == 0)
                {
                    txtUserOptionsPath.Text = Settings.UserOptionsPath;
                    return true;
                }

                var ret = MessageBox.Show(
                    "Do you really want to change path and reload settings?" +
                    "\r\n\r\nNew path will be:\r\n\r\n" + path,
                    "UserOptions path",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (ret == DialogResult.Yes)
                {
                    txtUserOptionsPath.Text = path;
                    Settings.UserOptionsPath = path;
                    mainForm.LoadUserOptions();
                }
                else
                {
                    txtUserOptionsPath.Text = Settings.UserOptionsPath;
                }
            }
            else
            {
                MessageBox.Show("Given path is invalid!", "UserOptions path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserOptionsPath.Text = Settings.UserOptionsPath;
                return false;
            }

            return true;
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            chooseFileDialog.FileName = Settings.UserOptionsFileName;
            chooseFileDialog.InitialDirectory = Settings.UserOptionsPath;
            chooseFileDialog.ShowDialog();
        }

        private void btnFromRegistry_Click(object sender, EventArgs e)
        {
            TryToParsePath(GetPathFromRegistry());
        }

        private void txtUserOptionsPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
                e.Handled = true;
            }
        }
        
        private void chooseFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            string path = chooseFileDialog.FileName;

            if (!path.EndsWith(Settings.UserOptionsFileName))
            {
                e.Cancel = true;
                return;
            }

            int index = path.IndexOf(Settings.UserOptionsFileName);
            
            path = path.Substring(0, index - 1);

            new Thread(() =>
            {
                Invoke(new TryToParsePathDelegate(TryToParsePath), path);
            }).Start();
        }
    }
}
