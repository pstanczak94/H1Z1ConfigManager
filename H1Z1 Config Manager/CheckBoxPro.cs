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
    public partial class CheckBoxPro : CheckBox
    {
        private string checkedState;
        private string uncheckedState;

        public string CheckedState
        {
            get { return checkedState; }
            set { checkedState = value; }
        }

        public string UncheckedState
        {
            get { return uncheckedState; }
            set { uncheckedState = value; }
        }

        public CheckBoxPro()
        {
            InitializeComponent();
            SetStates("1", "0");
        }

        public string GetState()
        {
            return Checked ? CheckedState : UncheckedState;
        }

        public void SetStates(string fChecked, string fUnchecked)
        {
            checkedState = fChecked;
            uncheckedState = fUnchecked;
        }

        public void ChangeStateByValue(string value)
        {
            if (string.Compare(checkedState, value, true) == 0)
                Checked = true;
            else if (string.Compare(uncheckedState, value, true) == 0)
                Checked = false;
        }
    }
}
