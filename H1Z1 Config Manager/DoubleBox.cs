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
    public partial class DoubleBox : TextBox
    {
        private double minimumValue;
        private double maximumValue;
        private double defaultValue;
        private string numberPattern;

        public DoubleBox()
        {
            minimumValue = 0.0;
            maximumValue = 1.0;
            defaultValue = 0.0;
            numberPattern = "0.0##";

            SetValue(defaultValue);

            InitializeComponent();
        }

        public override string Text
        {
            set { ValidateDouble(value); }
        }

        public double MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }

        public double MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        public double DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public string NumberPattern
        {
            get { return numberPattern; }
            set { numberPattern = value; }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            if (!ValidateDouble(Text))
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

            if (e.KeyChar == '.' || e.KeyChar == ',')
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
            if (m.Msg == WM_PASTE && !IsValidDouble(Clipboard.GetText()))
                return;

            base.WndProc(ref m);
        }

        public bool IsValidDouble(string text)
        {
            double value;

            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return true;

            return false;
        }

        public bool ValidateDouble(string text)
        {
            if (!string.IsNullOrWhiteSpace(text) && IsValidDouble(text))
                return SetValue(TextToDouble(text));

            SetValue(defaultValue);
            return false;
        }

        public double TextToDouble(string text)
        {
            double value;

            if (double.TryParse(text.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;

            return defaultValue;
        }

        public string DoubleToText(double value)
        {
            return value.ToString(numberPattern, CultureInfo.InvariantCulture);
        }

        public void SetTextSilent(string text)
        {
            base.Text = text;
        }

        public bool SetValue(double value)
        {
            if (value < MinimumValue)
            {
                SetTextSilent(DoubleToText(MinimumValue));
                return false;
            }

            if (value > MaximumValue)
            {
                SetTextSilent(DoubleToText(MaximumValue));
                return false;
            }

            SetTextSilent(DoubleToText(value));
            return true;
        }

        public double GetValue()
        {
            return TextToDouble(base.Text);
        }
    }
}
