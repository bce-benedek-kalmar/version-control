using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        private List<Ball> _balls = new List<Ball>();

        private BallFactory _factory;

        public BallFactory Factory
        {
            get { return _factory; }
            set { _factory = value; }
        }

        private void createTimer_Tick(object sender, EventArgs e)
        {
            Ball b = Factory.CreateNew();
            _balls.Add(b);
            mainPanel.Controls.Add(b);
            b.Left = -b.Width;
        }

        private void conveyorTimer_Tick(object sender, EventArgs e)
        {
            var rightestBallPosition = 0;

            foreach (var ball in _balls)
            {
                ball.MoveBall();

                if (ball.Left>rightestBallPosition)
                {
                    rightestBallPosition = ball.Left;
                }
            }

            if (rightestBallPosition>=1000)
            {
                var outBall = _balls[0];
                _balls.Remove(outBall);
                mainPanel.Controls.Remove(outBall);
            }
        }
    }
}
