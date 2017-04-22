using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class IntegerBox : TextBox
    {
        private int minimumValue;
        private int maximumValue;
        private int defaultValue;

        public IntegerBox()
        {
            minimumValue = -1000;
            maximumValue = 1000;
            defaultValue = 0;

            SetValue(defaultValue);

            InitializeComponent();
        }

        public override string Text
        {
            set { ValidateInteger(value); }
        }

        public int MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }

        public int MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        public int DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            if (!ValidateInteger(Text))
            {
                e.Cancel = true;
                SelectAll();
            }

            base.OnValidating(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            if (minimumValue < 0 && e.KeyChar == '-')
                e.Handled = false;

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
                e.Handled = true;
            }

            base.OnKeyPress(e);
        }

        const int WM_PASTE = 0x302;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTE && !IsValidInteger(Clipboard.GetText()))
                return;

            base.WndProc(ref m);
        }

        public bool IsValidInteger(string text)
        {
            int value;

            if (int.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return true;

            return false;
        }

        public bool ValidateInteger(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && IsValidInteger(text))
                return SetValue(TextToInteger(text));

            SetValue(defaultValue);
            return false;
        }

        public int TextToInteger(string text)
        {
            int value;

            if (int.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;

            return defaultValue;
        }

        public string IntegerToText(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetTextSilent(string text)
        {
            base.Text = text;
        }

        public bool SetPercentValue(double percent)
        {
            return SetValue((int)(percent * 100));
        }

        public bool SetValue(int value)
        {
            if (value < MinimumValue)
            {
                SetTextSilent(IntegerToText(MinimumValue));
                return false;
            }

            if (value > MaximumValue)
            {
                SetTextSilent(IntegerToText(MaximumValue));
                return false;
            }

            SetTextSilent(IntegerToText(value));
            return true;
        }

        public int GetValue()
        {
            return TextToInteger(base.Text);
        }
    }
}
