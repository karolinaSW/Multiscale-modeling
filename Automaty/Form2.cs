using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Automaty
{
    public partial class Form2 : Form
    {
        private System.Drawing.Graphics g;

        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        private System.Drawing.Pen pen_rect = new System.Drawing.Pen(Color.Green, 2);

        private SolidBrush brush_green = new SolidBrush(Color.Green);

        private System.Drawing.Pen pen_rect_1 = new System.Drawing.Pen(Color.Red, 2);

        private SolidBrush brush_red = new SolidBrush(Color.Red);

        public float a; // size of square cell

        public int incrementator = 1;

        Bitmap bm;




        public Grid_1D grid;
        public Rule rule;

        public Form2()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();





            //Image bp = new Bitmap(this.pictureBox1.Height, this.pictureBox1.Width);
            //pictureBox1.Image = bp;
            //Graphics g1 = Graphics.FromImage(bp);
            //this.DoubleBuffered = true;

            rule = new Rule(Form1.setRule);
            int r = rule.intBin;
            grid = new Grid_1D(Form1.setSizeX, r);


            //int w = pictureBox1.Width / (grid.getSize());
            //int h = pictureBox1.Height / (grid.getSize());

            //int cw = w / 2;
            //int ch = h / 2;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.DoubleBuffer,
                 true);

            //g.Clear(Color.White);



            int x, y;

           
            double xx = (Convert.ToDouble(pictureBox1.Width) / Convert.ToDouble(Form1.setSizeX));
            x = Convert.ToInt32(Math.Round(xx));
            double yy = (Convert.ToDouble(pictureBox1.Height) / Convert.ToDouble(Form1.setSizeY));
            y = Convert.ToInt32(Math.Round(yy));

            if (x < y)
            {
                a = pictureBox1.Width / Form1.setSizeX;
            }
            else
            {
                a = pictureBox1.Height / Form1.setSizeY;
            }


            bm = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(bm);


        }

        /*private void Draw()
        {
            

            int x, y;

            x = pictureBox1.Width / Form1.setSizeX;
            y = pictureBox1.Height / Form1.setSizeY;

            if (x < y)
            {
                a = (float)pictureBox1.Width / (float)Form1.setSizeX;
            }
            else
            {
                a = (float)pictureBox1.Height / (float)Form1.setSizeY;
            }


            //int w = pictureBox1.Width / (grid.getSize());
            //int h = pictureBox1.Height / (grid.getSize());

            for (int i = 0; i <= Form1.setSizeX; i++)
            {
                g.DrawLine(pen_line, (i * a), 0, i * a, Form1.setSizeY * a);

            }

            for(int i = 0; i <= Form1.setSizeY; i++)
            {
                g.DrawLine(pen_line, 0, i * a, Form1.setSizeX * a, i * a);

            }

            

            for (int i = 0; i < grid.getSize(); i++)
            {
                if (grid.currentArray[i].currentState == true)
                {
                    g.DrawRectangle(pen_rect, i * a, 0, (i + 1) * a, a);
                    g.FillRectangle(brush_green, i * a, 0, (i + 1) * a, a);
                }
                else
                {
                    g.DrawRectangle(pen_rect_1, i * a, 0, (i + 1) * a, a);
                    g.FillRectangle(brush_red, i * a, 0, (i + 1) * a, a);
                }
            }

        }*/


        private void button2_Click(object sender, EventArgs e)
        {

            for (int i = 0; i <= Form1.setSizeX; i++)
            {
                g.DrawLine(pen_line, i * a, 0, i * a, Form1.setSizeY * a);

            }

            for (int i = 0; i <= Form1.setSizeY; i++)
            {
                g.DrawLine(pen_line, 0, i * a, Form1.setSizeX * a, i * a);

            }

            for (int i = 0; i < grid.getSize(); i++)
            {
                if (grid.currentArray[i].currentState == true)
                {
                    g.FillRectangle(brush_green, i * a + 1, 0 + 1, a - 1, a - 1);
                }
                else
                {
                    g.FillRectangle(brush_red, i * a + 1, 0 + 1, a - 1, a - 1);
                }
            }

            pictureBox1.Image = bm;


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            //g = Graphics.FromImage(bm);
            if (incrementator < Form1.setSizeY)
            {
                grid = proceedStep(ref grid);



                for (int i = 0; i < grid.getSize(); i++)
                {
                    if (grid.currentArray[i].currentState == true)
                    {
                        //e.Graphics.DrawRectangle(pen_rect, i * a, 0, (i + 1) * a, a);
                        g.FillRectangle(brush_green, i * a + 1, a * incrementator + 1, a - 1, a - 1);
                    }
                    else
                    {
                        //e.Graphics.DrawRectangle(pen_rect_1, i * a, 0, (i + 1) * a, a);
                        g.FillRectangle(brush_red, i * a + 1, a * incrementator + 1, a - 1, a - 1);

                    }
                }
                incrementator++;
                //pictureBox1.Refresh();
                pictureBox1.Image = bm;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            /* 
             //pictureBox1.Invalidate();
             //Draw();





             for (int i = 0; i <= Form1.setSizeX; i++)
             {
                 e.Graphics.DrawLine(pen_line, i * a, 0, i * a, Form1.setSizeY * a);

             }

             for (int i = 0; i <= Form1.setSizeY; i++)
             {
                 e.Graphics.DrawLine(pen_line, 0, i * a, Form1.setSizeX * a, i * a);

             }

             for (int i = 0; i < grid.getSize(); i++)
             {
                 if (grid.currentArray[i].currentState == true)
                 {
                     //e.Graphics.DrawRectangle(pen_rect, i * a, 0, (i + 1) * a, a);
                     e.Graphics.FillRectangle(brush_green, i * a, 0, a, a);
                 }
                 else
                 {
                    //e.Graphics.DrawRectangle(pen_rect_1, i * a, 0, (i + 1) * a, a);
                    e.Graphics.FillRectangle(brush_red, i * a, 0, a, a);
                 }
             }

             //pictureBox1.Refresh();
             //pictureBox1.Invalidate();
             */
        }

        //private void pictureBox1_Paint(object sender, PaintEventArgs e)
        //{
        //draw();
        //}

        public Grid_1D proceedStep(ref Grid_1D gr)
        {
            //gr.currentArray = gr.newArray;

            for (int i = 0; i < gr.getSize(); i++)
            {

                gr.currentArray[i].currentState = gr.newArray[i].currentState;
                //gr.newArray[i].currentState = false;

                //this.newArray[i].currentState = false;
                //this.newArray[i].newState = false;
            }
            gr.checkNeighbours();
            gr.computeNewStates(gr.getR());
            return gr;
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            //g = Graphics.FromImage(bm);
            for (int j = 0; j < Form1.setSizeY -1; j++)
            {
                pictureBox1.Refresh();

                grid = proceedStep(ref grid);

                for (int i = 0; i < grid.getSize(); i++)
                {
                    if (grid.currentArray[i].currentState == true)
                    {
                        //e.Graphics.DrawRectangle(pen_rect, i * a, 0, (i + 1) * a, a);
                        g.FillRectangle(brush_green, i * a + 1, a * incrementator + 1, a - 1, a - 1);
                    }
                    else
                    {
                        //e.Graphics.DrawRectangle(pen_rect_1, i * a, 0, (i + 1) * a, a);
                        g.FillRectangle(brush_red, i * a + 1, a * incrementator + 1, a - 1, a - 1);

                    }
                }

                incrementator++;
                //pictureBox1.Refresh();

                pictureBox1.Image = bm;


                Thread.Sleep(10000/Form1.setSizeY);



            }

        }
    }
}
