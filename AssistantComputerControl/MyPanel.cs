using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssistantComputerControl {
    [System.ComponentModel.DesignerCategory("Code")]
    public class MyPanel : Panel {
        public Pen borderColor = Pens.Black;

        public MyPanel() {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e) {
            using (SolidBrush brush = new SolidBrush(BackColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);
            e.Graphics.DrawRectangle(borderColor, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }
    }
}
