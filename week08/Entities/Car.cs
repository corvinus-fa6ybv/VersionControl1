using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using week08.Abstractions;

namespace week08.Entities
{
    public class Car : Toy
    {
        protected override void DrawImage(Graphics g)
        {
            Image imageFile = Image.FromFile(@"C:\Users\juhas\Documents\Saját\Egyetem\5. Félév\IRF\VersionControl\week08\Image\car.png");
            g.DrawImage(imageFile, new Rectangle(0,0, Height,Width));
        }
    }
}
