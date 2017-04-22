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
    public partial class ListBoxPro : ListBox
    {
        private int lastSelectedIndex;
        private bool clickToUnselect;

        public bool ClickToUnselect
        {
            get { return clickToUnselect; }
            set { clickToUnselect = value; }
        }

        public ListBoxPro()
        {
            InitializeComponent();

            lastSelectedIndex = ListBox.NoMatches;
            clickToUnselect = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int index = IndexFromPoint(e.Location);

            if (index == ListBox.NoMatches && SelectedIndex != ListBox.NoMatches)
                ClearSelected();

            base.OnMouseClick(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            int index = SelectedIndex;

            if (index == ListBox.NoMatches)
                lastSelectedIndex = ListBox.NoMatches;
            else
            {
                if (clickToUnselect && index == lastSelectedIndex)
                    ClearSelected();
                else
                    lastSelectedIndex = index;
            }

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0 && e.Index < Items.Count)
            {
                object item = Items[e.Index];
                e.DrawBackground();
                e.DrawFocusRectangle();
                Brush brush = new SolidBrush(e.ForeColor);
                SizeF size = e.Graphics.MeasureString(item.ToString(), e.Font);
                e.Graphics.DrawString(item.ToString(), e.Font, brush,
                    e.Bounds.Left + (e.Bounds.Width / 2 - size.Width / 2),
                    e.Bounds.Top + (e.Bounds.Height / 2 - size.Height / 2));
            }

            base.OnDrawItem(e);
        }
    }
}
