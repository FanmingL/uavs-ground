using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uavs
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int xTarget = Int16.Parse(this.textBox6.Text) * pictureBox1.Width / 3600,
            yTarget = Int16.Parse(this.textBox7.Text) * pictureBox1.Height / 2400;
            DrawTarget(xTarget, yTarget);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            DrawGRID();
        }
        public void DrawTarget(int x, int y)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Pen mypen = new Pen(Color.Red, 2);
            //  int xTarget = Int16.Parse(this.textBox6.Text) * pictureBox1.Width / 3600,
            // yTarget = Int16.Parse(this.textBox7.Text) * pictureBox1.Height / 2400;
            g.DrawLine(mypen, x, y + 10, x, y - 10);
            g.DrawLine(mypen, x + 10, y, x - 10, y);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        public void DrawGRID()
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Pen mypen = new Pen(Color.Black, 1);
            mypen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            mypen.DashPattern = new float[] { 5, 5 };
        //    g.Clear(Color.White);
            for (int i = 1; i < 36; i += 1)
            {
                g.DrawLine(mypen, i * 100 / 3600.0f * this.pictureBox1.Width, 0, i * 100 / 3600.0f * this.pictureBox1.Width,
                    this.pictureBox1.Height);
            }
            for (int i = 1; i < 24; i += 1)
            {
                g.DrawLine(mypen, 0, i * 100 / 2400.0f * this.pictureBox1.Height, this.pictureBox1.Width,
                   i * 100 / 2400.0f * this.pictureBox1.Height);
            }

        }

        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            DrawGRID();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawGRID();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            DrawGRID();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DrawGRID();
        }
        public void Init(Graphics g, PictureBox p)
        {
            Pen mypen = new Pen(Color.Red, 2);
            //  g.Clear(Color.White);
            DrawGRID();
            Point point1 = new Point(944 * p.Width / 3600, 922 * p.Height / 2400),
                point2 = new Point(2656 * p.Width / 3600, 922 * p.Height / 2400),
                point3 = new Point(1271 * p.Width / 3600, 1928 * p.Height / 2400),
                point4 = new Point(1800 * p.Width / 3600, 300 * p.Height / 2400),
                point5 = new Point(2329 * p.Width / 3600, 1928 * p.Height / 2400);
            Point point6 = point1;
            Point[] path = new Point[6];
            path[0] = point1;
            path[1] = point2;
            path[2] = point3;
            path[3] = point4;
            path[4] = point5;
            path[5] = point6;
            g.DrawLines(mypen, path);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = this.pictureBox1.CreateGraphics();
            Init(g,this.pictureBox1);

        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {

        }
    }
}
