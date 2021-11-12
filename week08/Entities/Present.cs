using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using week08.Abstractions;

namespace week08.Entities
{
    public class Present : Toy
    {
        public SolidBrush Box { get; private set; }
        public SolidBrush Ribbon1 { get; private set; }
        public SolidBrush Ribbon2 { get; private set; }

        public Present (Color ribbon, Color box)
        {
            Box = new SolidBrush(box);
            Ribbon1 = new SolidBrush(ribbon);
            Ribbon2 = new SolidBrush(ribbon);

        }
        protected override void DrawImage(Graphics g)
        {
          g.FillRectangle(Box,0, 0, Width, Height);
            g.FillRectangle(Ribbon1, Height/5,0, Width/5, Height);
            g.FillRectangle(Ribbon2, 0, Width/5, Width,  Height/5);

        }
    }
}
