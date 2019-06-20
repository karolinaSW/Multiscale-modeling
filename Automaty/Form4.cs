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
    public partial class Form4 : Form
    {
        public static int setNeighbours = 8;

        public static int setSizeX = 0;
        public static int setSizeY = 0;
        public static int setSteps = 0;

        public int incrementator = 0;
        public int inc = 0;

        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        //private System.Drawing.Pen pen_rect = new System.Drawing.Pen(Color.Green, 2);
        private SolidBrush brush_grey = new SolidBrush(Color.LightSteelBlue);
        //private System.Drawing.Pen pen_rect_1 = new System.Drawing.Pen(Color.Red, 2);
        private SolidBrush brush_blue = new SolidBrush(Color.MidnightBlue);
        public float a; // size of square cell
        //public int incrementator = 1;
        Bitmap bm;

        public Grid_2D grid;

        public Form4()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);
        }

        public void DrawGrid(ref int setSizeX, ref int setSizeY, ref float a)
        {
            g.Clear(Color.White);
            pictureBox1.Refresh();

            setSizeX = Convert.ToInt32(textBox1.Text);
            setSizeY = Convert.ToInt32(textBox2.Text);


            int x, y;

            double xx = (Convert.ToDouble(pictureBox1.Width) / Convert.ToDouble(setSizeX));
            x = Convert.ToInt32(Math.Round(xx));
            double yy = (Convert.ToDouble(pictureBox1.Height) / Convert.ToDouble(setSizeY));
            y = Convert.ToInt32(Math.Round(yy));

            if (x < y)
            {
                a = pictureBox1.Width / setSizeX;
            }
            else
            {
                a = pictureBox1.Height / setSizeY;
            }


            for (int i = 0; i <= setSizeX; i++)
            {
                g.DrawLine(pen_line, i * a, 0, i * a, setSizeY * a);

            }

            for (int i = 0; i <= setSizeY; i++)
            {
                g.DrawLine(pen_line, 0, i * a, setSizeX * a, i * a);

            }
            pictureBox1.Image = bm;

            //

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.DoubleBuffer,
                 true);

            //g.Clear(Color.White);




            bm = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(bm);


        }

        public Grid_2D proceedStep(ref Grid_2D gr)
        {
            //gr.currentArray = gr.newArray;

            for (int i = 0; i < gr.getSizeX(); i++)
            {
                for (int j = 0; j < gr.getSizeY(); j++)
                {

                    gr.currentArray[i, j].currentState = gr.newArray[i, j].currentState;
                    //gr.newArray[i].currentState = false;

                    //this.newArray[i].currentState = false;
                    //this.newArray[i].newState = false;
                }
            }
            checkNeighbours(ref gr);
            computeNewStates(ref gr);
            return gr;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);

            grid = new Grid_2D(setSizeX, setSizeY);

            int startX = 3, startY = 3;

            FunUnchangeable(ref grid, startX, startY);

            computeNewStates(ref grid);
            checkNeighbours(ref grid);



        }

        public void FunUnchangeable(ref Grid_2D grid, int startX, int startY)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            switch (randomNumber)
            {
                case 0:
                    {
                        grid.currentArray[startX, startY].currentState = true;
                        grid.currentArray[startX, startY + 1].currentState = true;
                        grid.currentArray[startX + 1, startY].currentState = true;
                        grid.currentArray[startX + 1, startY + 1].currentState = true;
                        break;
                    }
                case 1:
                    {
                        grid.currentArray[startX, startY].currentState = true;
                        grid.currentArray[startX + 1, startY + 1].currentState = true;
                        grid.currentArray[startX, startY + 2].currentState = true;
                        grid.currentArray[startX - 1, startY + 1].currentState = true;
                        break;
                    }
                case 2:
                    {
                        grid.currentArray[startX, startY].currentState = true;
                        grid.currentArray[startX + 1, startY].currentState = true;
                        grid.currentArray[startX + 2, startY + 1].currentState = true;
                        grid.currentArray[startX + 2, startY + 2].currentState = true;
                        grid.currentArray[startX - 1, startY + 1].currentState = true;
                        grid.currentArray[startX - 1, startY + 2].currentState = true;
                        grid.currentArray[startX, startY + 3].currentState = true;
                        grid.currentArray[startX + 1, startY + 3].currentState = true;
                        break;
                    }

            }

            for (int i = 0; i < grid.getSizeX(); i++)
            {
                for (int j = 0; j < grid.getSizeY(); j++)
                {
                    if (grid.currentArray[i, j].currentState == true)
                    {
                        g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                    else
                    {
                        g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }
        }

        //

        public void FunOscillator(ref Grid_2D grid, int startX, int startY)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 2);

            randomNumber = 0;///////////////////////////////////////////
            switch (randomNumber)
            {
                case 0:
                    {
                        grid.currentArray[startX, startY].currentState = true;
                        grid.currentArray[startX, startY + 1].currentState = true;
                        grid.currentArray[startX, startY + 2].currentState = true;

                        break;
                    }
                case 1:
                    {
                        grid.currentArray[startX, startY].currentState = true;
                        grid.currentArray[startX, startY + 1].currentState = true;
                        grid.currentArray[startX, startY + 2].currentState = true;
                        grid.currentArray[startX + 1, startY + 1].currentState = true;
                        grid.currentArray[startX + 1, startY + 2].currentState = true;
                        grid.currentArray[startX + 1, startY + 3].currentState = true;
                        break;
                    }


            }


            for (int i = 0; i < grid.getSizeX(); i++)
            {
                for (int j = 0; j < grid.getSizeY(); j++)
                {
                    if (grid.currentArray[i, j].currentState == true)
                    {
                        g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                    else
                    {
                        g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }
        }

        public void FunGlider(ref Grid_2D grid, int startX, int startY)
        {

            grid.currentArray[startX + 1, startY].currentState = true;
            grid.currentArray[startX + 2, startY + 1].currentState = true;
            grid.currentArray[startX, startY + 2].currentState = true;
            grid.currentArray[startX + 1, startY + 2].currentState = true;
            grid.currentArray[startX + 2, startY + 2].currentState = true;



            for (int i = 0; i < grid.getSizeX(); i++)
            {
                for (int j = 0; j < grid.getSizeY(); j++)
                {
                    if (grid.currentArray[i, j].currentState == true)
                    {
                        g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                    else
                    {
                        g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }
        }

        public void FunRandom(ref Grid_2D grid)
        {
            Random random = new Random();
            

            for (int i = 0; i < grid.getSizeX(); i++)
            {
                for (int j = 0; j < grid.getSizeY(); j++)
                {
                    int randomNumber = random.Next(0, 3);
                    bool val = Convert.ToBoolean(randomNumber);
                    grid.currentArray[i, j].currentState = val;
                }
            }



            for (int i = 0; i < grid.getSizeX(); i++)
            {
                for (int j = 0; j < grid.getSizeY(); j++)
                {
                    if (grid.currentArray[i, j].currentState == true)
                    {
                        g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                    else
                    {
                        g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }
        }

        public void checkNeighbours(ref Grid_2D g)
        {

            for (int i = 0; i < g.getSizeX(); i++)
            {
                for (int j = 0; j < g.getSizeY(); j++)
                {
                    /*
                    g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                    g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                    */

                    if (i == 0 && j == 0)
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[g.getSizeX() - 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[g.getSizeX() - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[g.getSizeX() - 1, g.getSizeY() - 1].currentState;

                    }
                    else if (i == 0 && j == g.getSizeY() - 1)
                    {
                        //g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[g.getSizeX() - 1, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[g.getSizeX() - 1, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[g.getSizeX() - 1, j - 1].currentState;
                    }
                    else if (i == g.getSizeX() - 1 && j == 0)
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[0, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[0, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i, g.getSizeY() - 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, g.getSizeY() - 1].currentState;
                    }
                    else if (i == g.getSizeX() - 1 && j == g.getSizeY() - 1)
                    {
                        //g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[0, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[0, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[0, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, 0].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                    }
                    else if (i == 0 && j != 0 && j != g.getSizeY() - 1)
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[g.getSizeX() - 1, j].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[g.getSizeX()-1, j + 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[g.getSizeX() - 1, j - 1].currentState;

                    }
                    else if (i == g.getSizeX() - 1 && j != 0 && j != g.getSizeY() - 1)
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[0, j].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[0, j].currentState;

                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[0, j - 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[0, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                    }

                    else if (j == 0 && i != 0 && i != g.getSizeX() - 1)
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, g.getSizeY() - 1].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;

                        //g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, g.getSizeY() - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, g.getSizeY() -1].currentState;
                    }
                    else if (j == g.getSizeY() - 1 && i != 0 && i != g.getSizeX() - 1)
                    {
                        //g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, 0].currentState;

                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, 0].currentState;
                        //g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, 0].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                    }
                    else
                    {
                        g.currentArray[i, j].currentStateOfNeighbours[0] = g.currentArray[i, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[1] = g.currentArray[i + 1, j - 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[2] = g.currentArray[i + 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[3] = g.currentArray[i + 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[4] = g.currentArray[i, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[5] = g.currentArray[i - 1, j + 1].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[6] = g.currentArray[i - 1, j].currentState;
                        g.currentArray[i, j].currentStateOfNeighbours[7] = g.currentArray[i - 1, j - 1].currentState;
                    }

                }
            }


            for (int i = 0; i < g.getSizeX(); i++)
            {
                for (int j = 0; j < g.getSizeY(); j++)
                {
                    g.currentArray[i, j].numberOfAliveNeighbours = 0;
                    g.currentArray[i, j].numberOfDeadNeighbours = 0;

                    for (int k = 0; k < 8; k++)
                    {
                        if (g.currentArray[i, j].currentStateOfNeighbours[k] == true)
                        {
                            g.currentArray[i, j].numberOfAliveNeighbours++;
                        }
                        else
                            g.currentArray[i, j].numberOfDeadNeighbours++;
                    }
                }
            }



        }

        public void computeNewStates(ref Grid_2D g)
        {
            bool val;
            for (int i = 0; i < g.getSizeX(); i++)
            {
                for (int j = 0; j < g.getSizeY(); j++)
                {
                    val = g.currentArray[i, j].currentState;

                    if (g.currentArray[i, j].currentState == true && g.currentArray[i, j].numberOfAliveNeighbours > 3)
                    {
                        val = false;
                    }
                    else if (g.currentArray[i, j].currentState == true && (g.currentArray[i, j].numberOfAliveNeighbours == 2 || g.currentArray[i, j].numberOfAliveNeighbours == 3))
                    {
                        val = true;
                    }
                    else if (g.currentArray[i, j].currentState == true && g.currentArray[i, j].numberOfAliveNeighbours < 2)
                    {
                        val = false;
                    }
                    else if (g.currentArray[i, j].currentState == false && g.currentArray[i, j].numberOfAliveNeighbours == 3)
                    {
                        val = true;
                    }

                    g.newArray[i, j].currentState = val;
                    g.newArray[i, j].numberOfAliveNeighbours = 0;
                    g.newArray[i, j].numberOfDeadNeighbours = 0;


                }

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            incrementator = 0;
            /*
            for (int k = 0; k < Convert.ToInt32(textBox3.Text); k++)
            {
                pictureBox1.Refresh();
                DrawGrid(ref setSizeX, ref setSizeY);

                //grid = new Grid_2D(setSizeX, setSizeY);

                grid = proceedStep(ref grid);

                for (int i = 0; i < grid.getSizeX() - 1; i++)
                {
                    for (int j = 0; j <= grid.getSizeY() - 1; j++)
                    {
                        if (grid.currentArray[i, j].currentState == true)
                        {
                            g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                        else
                        {
                            g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                    }
                }

                //incrementator++;
                pictureBox1.Refresh();

                pictureBox1.Image = bm;


                //Thread.Sleep(10000 / Form4.setSizeY);
                //Thread.Sleep(500);
            }
            //incrementator = 0;

            


            pictureBox1.Refresh();
            //g = Graphics.FromImage(bm);
            if (incrementator < Convert.ToInt32(textBox3.Text))
            {
                grid = proceedStep(ref grid);



                for (int i = 0; i < grid.getSizeX(); i++)
                {
                    for (int j = 0; j < grid.getSizeY(); j++)
                    {
                        if (grid.currentArray[i,j].currentState == true)
                        {
                            //e.Graphics.DrawRectangle(pen_rect, i * a, 0, (i + 1) * a, a);
                            g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                        else
                        {
                            //e.Graphics.DrawRectangle(pen_rect_1, i * a, 0, (i + 1) * a, a);
                            g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);

                        }
                    }
                }
                incrementator++;
                //pictureBox1.Refresh();
                pictureBox1.Image = bm;

            }*/
            while (incrementator <= Convert.ToInt32(textBox3.Text))
            {
                grid = proceedStep(ref grid);

                DrawGrid(ref setSizeX, ref setSizeY, ref a);

                for (int i = 0; i < grid.getSizeX(); i++)
                {
                    for (int j = 0; j < grid.getSizeY(); j++)
                    {
                        if (grid.currentArray[i, j].currentState == true)
                        {
                            g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                        else
                        {
                            g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                    }
                }

                checkNeighbours(ref grid);
                computeNewStates(ref grid);

                Thread.Sleep(300);

                incrementator++;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);

            grid = new Grid_2D(setSizeX, setSizeY);

            int startX = 3, startY = 3;

            FunOscillator(ref grid, startX, startY);

            checkNeighbours(ref grid);
            computeNewStates(ref grid);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);

            grid = new Grid_2D(setSizeX, setSizeY);

            int startX = 3, startY = 3;

            FunGlider(ref grid, startX, startY);

            checkNeighbours(ref grid);
            computeNewStates(ref grid);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);

            grid = new Grid_2D(setSizeX, setSizeY);

            //int startX = 3, startY = 3;

            FunRandom(ref grid);

            checkNeighbours(ref grid);
            computeNewStates(ref grid);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            inc = incrementator;
            incrementator = 0;
        }

        private void textBox5_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            float x = (MousePosition.X - 13);
            float y = (MousePosition.Y - 13);

            int xx = Convert.ToInt32(x /a);
            int yy = Convert.ToInt32(y /a);

            textBox5.Text = xx.ToString();
            textBox4.Text = yy.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            incrementator = inc;
            while (incrementator <= Convert.ToInt32(textBox3.Text))
            {
                grid = proceedStep(ref grid);

                DrawGrid(ref setSizeX, ref setSizeY, ref a);

                for (int i = 0; i < grid.getSizeX(); i++)
                {
                    for (int j = 0; j < grid.getSizeY(); j++)
                    {
                        if (grid.currentArray[i, j].currentState == true)
                        {
                            g.FillRectangle(brush_blue, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                        else
                        {
                            g.FillRectangle(brush_grey, i * a + 1, j * a + 1, a - 1, a - 1);
                        }
                    }
                }

                checkNeighbours(ref grid);
                computeNewStates(ref grid);

                Thread.Sleep(300);

                

                incrementator++;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
