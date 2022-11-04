using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Workshop.Abstractions;
using Workshop.Entities;

namespace Workshop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Factory = new BallFactory();
        }

        private List<Toy> _toys = new List<Toy>();

        private IToyFactory _factory;

        public IToyFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            Toy t = Factory.CreateNew();
            _toys.Add(t);
            mainPanel.Controls.Add(t);
            t.Left = -t.Width;
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var rightestToyPosition = 0;

            foreach (var toy in _toys)
            {
                toy.MoveToy();

                if (toy.Left>rightestToyPosition)
                {
                    rightestToyPosition = toy.Left;
                }
            }

            if (rightestToyPosition>=1000)
            {
                var outToy = _toys[0];
                _toys.Remove(outToy);
                mainPanel.Controls.Remove(outToy);
            }
        }
    }
}
