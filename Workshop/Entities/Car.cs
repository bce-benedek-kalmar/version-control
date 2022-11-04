using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workshop.Abstractions;

namespace Workshop.Entities
{
    class Car : Toy
    {
        protected override void DrawImage(Graphics graphics)
        {
            Image imagefile = Image.FromFile("Images/car.png");
            graphics.DrawImage(imagefile, new Rectangle(0, 0, Width, Height));
        }
    }
}
