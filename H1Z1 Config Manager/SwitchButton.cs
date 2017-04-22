using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H1Z1_Config_Manager
{
    public partial class SwitchButton : Button
    {
        public class SwitchButtonState
        {
            public string key, value;

            public SwitchButtonState(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }

        public class SwitchButtonStates : List<SwitchButtonState>
        {
            public void AddState(string key, string value)
            {
                Add(new SwitchButtonState(key, value));
            }

            public void ParseStates(string text, bool clearStates)
            {
                if (clearStates)
                    Clear();

                string[] states = text.Split('|');

                foreach (string state in states)
                {
                    string[] keyAndValue = state.Split(':');

                    if (keyAndValue.Length == 2)
                        AddState(keyAndValue[0].Trim(), keyAndValue[1].Trim());
                    else
                        AddState(keyAndValue[0].Trim(), keyAndValue[0].Trim());
                }
            }
        }

        private SwitchButtonStates switchStates;
        private ToolTip stateTooltip;
        private int currentStateIndex;
        private bool stateTooltipEnabled;

        public int CurrentStateIndex
        {
            get { return currentStateIndex; }
            set { ChangeStateByIndex(value); }
        }

        public bool StateTooltipEnabled
        {
            get { return stateTooltipEnabled; }
            set { stateTooltipEnabled = value; }
        }

        public SwitchButton()
        {
            InitializeComponent();

            switchStates = new SwitchButtonStates();
            stateTooltip = null;
            currentStateIndex = 0;
            stateTooltipEnabled = true;
        }

        protected override void OnClick(EventArgs e)
        {
            if (e.GetType() != typeof(MouseEventArgs))
                ChangeStateForward();

            base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Right)
                mevent = new MouseEventArgs(MouseButtons.Left, mevent.Clicks, mevent.X, mevent.Y, mevent.Delta);

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            MouseEventArgs eventToSend = mevent;

            if (mevent.Button == MouseButtons.Right)
                eventToSend = new MouseEventArgs(MouseButtons.Left, mevent.Clicks, mevent.X, mevent.Y, mevent.Delta);

            base.OnMouseUp(eventToSend);

            if (mevent.Button == MouseButtons.Left && ClientRectangle.Contains(mevent.Location))
                ChangeStateForward();
            else if (mevent.Button == MouseButtons.Right && ClientRectangle.Contains(mevent.Location))
                ChangeStateBack();
        }

        public void ChangeStateForward()
        {
            bool isLast = (currentStateIndex + 1 >= switchStates.Count);
            int nextIndex = isLast ? 0 : currentStateIndex + 1;
            ChangeStateByIndex(nextIndex);
        }

        public void ChangeStateBack()
        {
            bool isFirst = (currentStateIndex == 0);
            int nextIndex = isFirst ? switchStates.Count - 1 : currentStateIndex - 1;
            ChangeStateByIndex(Math.Max(nextIndex, 0));
        }

        public void ChangeStateByKey(string stateKey)
        {
            int len = switchStates.Count;
            for (int i = 0; i < len; i++)
            {
                if (string.Compare(switchStates[i].key, stateKey, true) == 0)
                {
                    ChangeStateByIndex(i);
                    break;
                }
            }
        }

        public void ChangeStateByValue(string stateValue)
        {
            int len = switchStates.Count;
            for (int i = 0; i < len; i++)
            {
                if (string.Compare(switchStates[i].value, stateValue, true) == 0)
                {
                    ChangeStateByIndex(i);
                    break;
                }
            }
        }

        public void ChangeStateByValue(int stateValue)
        {
            int len = switchStates.Count;
            for (int i = 0; i < len; i++)
            {
                if (Tools.StrToInt(switchStates[i].value) == stateValue)
                {
                    ChangeStateByIndex(i);
                    break;
                }
            }
        }

        public void ChangeStateByValue(double stateValue)
        {
            int len = switchStates.Count;
            for (int i = 0; i < len; i++)
            {
                if (Tools.StrToDouble(switchStates[i].value) == stateValue)
                {
                    ChangeStateByIndex(i);
                    break;
                }
            }
        }

        public void ChangeStateByIndex(int index)
        {
            if (index >= 0 && index < switchStates.Count)
            {
                currentStateIndex = index;
                Text = switchStates[index].key;

                if (stateTooltip != null)
                {
                    stateTooltip.Dispose();
                    stateTooltip = null;
                }

                if (stateTooltipEnabled)
                {
                    stateTooltip = new ToolTip();
                    stateTooltip.SetToolTip(this, "Value: " + switchStates[index].value);
                }
            }
        }

        public void AddState(string key, string value)
        {
            switchStates.AddState(key, value);
        }

        public void InitStates(string text)
        {
            switchStates.ParseStates(text, true);
        }

        public void InitStates(string text, string stateToInit)
        {
            switchStates.ParseStates(text, true);
            ChangeStateByKey(stateToInit);
        }

        public string GetStateToString(string defaultValue = "")
        {
            if (currentStateIndex >= 0 && currentStateIndex < switchStates.Count)
                return switchStates[currentStateIndex].value;

            return defaultValue;
        }

        public int GetStateToInt(int defaultValue = 0)
        {
            if (currentStateIndex >= 0 && currentStateIndex < switchStates.Count)
                return Tools.StrToInt(switchStates[currentStateIndex].value, defaultValue);

            return defaultValue;
        }

        public double GetStateToDouble(double defaultValue = 0.0)
        {
            if (currentStateIndex >= 0 && currentStateIndex < switchStates.Count)
                return Tools.StrToDouble(switchStates[currentStateIndex].value, defaultValue);

            return defaultValue;
        }
    }
}
