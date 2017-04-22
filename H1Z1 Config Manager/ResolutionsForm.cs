using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class ResolutionsForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        const int ENUM_CURRENT_SETTINGS = -1;
        const int ENUM_REGISTRY_SETTINGS = -2;

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        public class ResolutionInfo
        {
            public short Width, Height, RefreshRate;

            public ResolutionInfo(short width, short height, short refreshRate)
            {
                this.Width = width;
                this.Height = height;
                this.RefreshRate = refreshRate;
            }

            public override string ToString()
            {
                return Width + " x " + Height;
            }
        }

        public List<ResolutionInfo> Resolutions;
        private MainForm mainForm;

        public ResolutionsForm(MainForm parent)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            mainForm = parent;

            Resolutions = new List<ResolutionInfo>();

            buttonAccept.Enabled = false;

            LoadAvailableResolutions();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (listBoxResolutions.SelectedIndex == ListBox.NoMatches)
                return;

            ResolutionInfo info = listBoxResolutions.SelectedItem as ResolutionInfo;

            ResolutionHasBeenPicked(info);
        }

        private void comboBoxRefreshRates_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ResolutionInfo> SortedList = Resolutions
                .OrderByDescending(o => o.Width)
                .ThenByDescending(o => o.Height)
                .ToList();

            listBoxResolutions.ClearSelected();
            listBoxResolutions.Items.Clear();

            foreach (ResolutionInfo info in SortedList)
                if (info.RefreshRate.ToString() == cmbRefreshRates.Text)
                    listBoxResolutions.Items.Add(info);
        }

        private void listBoxResolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxResolutions.SelectedIndex == ListBox.NoMatches)
                buttonAccept.Enabled = false;
            else
                buttonAccept.Enabled = true;
        }

        private void listBoxResolutions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxResolutions.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
                ResolutionHasBeenPicked(listBoxResolutions.Items[index] as ResolutionInfo);
        }
        
        public void LoadAvailableResolutions()
        {
            DEVMODE vDevMode = new DEVMODE();
            int i = 0;

            while (EnumDisplaySettings(null, i, ref vDevMode))
            {
                short resWidth = (short)vDevMode.dmPelsWidth;
                short resHeight = (short)vDevMode.dmPelsHeight;
                short refreshRate = (short)vDevMode.dmDisplayFrequency;

                if (!Resolutions.Exists(o => o.Width == resWidth && o.Height == resHeight && o.RefreshRate == refreshRate))
                    Resolutions.Add(new ResolutionInfo(resWidth, resHeight, refreshRate));

                i++;
            }

            ComboBox.ObjectCollection items = cmbRefreshRates.Items;

            items.Clear();

            List<ResolutionInfo> SortedList = Resolutions
                .OrderByDescending(o => o.RefreshRate)
                .ToList();

            foreach (ResolutionInfo info in SortedList)
                if (!items.Contains(info.RefreshRate))
                    items.Add(info.RefreshRate);

            if (items.Count > 0)
            {
                int index = 0;

                for (i = 0; i < items.Count; i++)
                {
                    if ((short)items[i] == 60)
                    {
                        index = i;
                        break;
                    }
                }

                cmbRefreshRates.SelectedIndex = index;
            }
        }

        protected void ResolutionHasBeenPicked(ResolutionInfo info)
        {
            mainForm.UserHasChoosenResolution(info);
            Close();
        }
    }
}
