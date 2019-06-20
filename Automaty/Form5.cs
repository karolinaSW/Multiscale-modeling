using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automaty
{
    public partial class Form5 : Form
    {


        public static int setSizeX = 0;
        public static int setSizeY = 0;
        public float a; // size of square cell
        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        Bitmap bm;
        public static int numberOfNeighbours = 0;
        public static bool flagDynamicNeighbour = false;
        public static float radius = 0;
        public static bool periodic;
        public static int nukleationTag;
        public static int ilosc_x, ilosc_y, ilosc, r;
        public static int numberOfGrains;
        public Color[] grainColorTable;
        private Random rnd = new Random();
        GridGrains grid;




        public Form5()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            DrawGrid(ref setSizeX, ref setSizeY, ref a);
            textBox8.Text = "";
        }

        public void DrawGrid(ref int setSizeX, ref int setSizeY, ref float a)
        {
            g.Clear(Color.White);
            pictureBox1.Refresh();

            setSizeX = Convert.ToInt32(textBox10.Text);
            setSizeY = Convert.ToInt32(textBox9.Text);


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

            grid = new GridGrains(setSizeX, setSizeY);
            int k = 0;
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j].index = k;
                    k++;
                }
            }

        }

        private void Form5_Load(object sender, EventArgs e)
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e) //accept data
        {
            if (radioButton1.Checked == true)
            {
                numberOfNeighbours = 4;
            }
            if (radioButton2.Checked == true)
            {
                numberOfNeighbours = 8;
            }
            if (radioButton3.Checked == true)
            {
                numberOfNeighbours = 5;
            }
            if (radioButton4.Checked == true)
            {
                numberOfNeighbours = 6;
            }
            if (radioButton5.Checked == true && textBox11 != null)
            {
                flagDynamicNeighbour = true;
            }
            if (checkBox1.CheckState == CheckState.Checked)
            {
                periodic = true;
            }
            if (radioButton6.Checked == true)
            {
                nukleationTag = 1;
                ilosc_x = Convert.ToInt32(textBox1.Text);
                ilosc_y = Convert.ToInt32(textBox2.Text);
            }
            if (radioButton7.Checked == true)
            {
                nukleationTag = 2;
                ilosc = Convert.ToInt32(textBox3.Text);
                r = Convert.ToInt32(textBox4.Text);
            }
            if (radioButton8.Checked == true)
            {
                nukleationTag = 3;
                ilosc = Convert.ToInt32(textBox5.Text);
            }
            if (radioButton9.Checked == true)
            {
                nukleationTag = 4;
            }

            Nucleation(nukleationTag);
            pictureBox1.Refresh();

        }

        public void Nucleation(int t)
        {
            switch (t)
            {
                case 1:
                    {
                        if (setSizeX - ilosc_x < 0)
                        {
                            textBox8.Text = "Nie można wyświetlić wszystkich zadanych zarodków ziaren.";
                            ilosc_x = setSizeX;
                        }
                        if (setSizeY - ilosc_y < 0)
                        {
                            textBox8.Text = "Nie można wyświetlić wszystkich zadanych zarodków ziaren.";
                            ilosc_y = setSizeY;
                        }

                        numberOfGrains = ilosc_x * ilosc_y;
                        setColorTable(numberOfGrains);

                        int odstX, odstY;
                        if (ilosc_x == 1)
                        {
                            odstX = 0;
                        }
                        else
                        {
                            odstX = (setSizeX - ilosc_x) / (ilosc_x - 1);

                        }

                        if (ilosc_y == 1)
                        {
                            odstY = 0;
                        }
                        else
                        {
                            odstY = (setSizeY - ilosc_y) / (ilosc_y - 1);

                        }


                        int index = 0;
                        for (int i = 0; i < setSizeX; i = i + odstX + 1)
                        {
                            int indY = 0;
                            for (int j = 0; j < setSizeY; j = j + odstY + 1)
                            {
                                if (indY >= ilosc_y)
                                {
                                    break;
                                }
                                if (index >= ilosc_x * ilosc_y)
                                {
                                    break;
                                }
                                grid.currentArray[i, j].currentState = index;
                                g.FillRectangle(new SolidBrush(grainColorTable[index]), i * a + 1, j * a + 1, a - 1, a - 1);
                                index++;
                                indY++;
                            }
                        }

                        break;
                    }
                case 2:
                    {
                        setColorTable(ilosc);
                        //int []tabReservedIndexes;
                        List<int> reservedIndexes = new List<int>();

                        int firstCellX = Convert.ToInt32(Math.Ceiling((r - (a / 2)) / a)) + 1;
                        int firstCellY = firstCellX;
                        float centerX = (firstCellX * a) - (a / 2);
                        float centerY = centerX;
                        //g.DrawEllipse(new Pen(Color.Red, 1), centerX - 1, centerY - 1, 2, 2);
                        //g.DrawLine(new Pen(Color.DarkBlue, 1), centerX, centerY, centerX + r, centerY);
                        //g.DrawEllipse(new Pen(Color.Red, 1), centerX - r, centerY - r, r + r, r + r);

                        int seedNr = 1;

                        for (int i = 0; i < setSizeX; i++)//2 * firstCellX - 1; i++) //indeksy
                        {
                            for (int j = 0; j < setSizeY; j++)//2 * firstCellY - 1; j++)
                            {
                                if (i >= setSizeX || j >= setSizeY)
                                {
                                    break;
                                }
                                if (Math.Pow((i * a - centerX), 2) + Math.Pow(((j + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                    || Math.Pow(((i + 1) * a - centerX), 2) + Math.Pow(((j + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                    || Math.Pow((i * a - centerX), 2) + Math.Pow((j * a - centerY), 2) <= Math.Pow(r, 2)
                                    || Math.Pow(((i + 1) * a - centerX), 2) + Math.Pow((j * a - centerY), 2) <= Math.Pow(r, 2))
                                {
                                    if(!reservedIndexes.Contains(grid.currentArray[i, j].index))
                                    {
                                        reservedIndexes.Add(grid.currentArray[i, j].index);

                                    }
                                    g.FillRectangle(new SolidBrush(Color.LightCoral), i * a + 1, j * a + 1, a - 1, a - 1);

                                }
                            }
                        }
                        grid.currentArray[firstCellX - 1, firstCellY - 1].currentState = seedNr;
                        g.FillRectangle(new SolidBrush(grainColorTable[seedNr-1]), (firstCellX-1) * a + 1, (firstCellY - 1) * a + 1, a - 1, a - 1);

                        seedNr++;
                        g.DrawEllipse(new Pen(Color.Red, 1), centerX - 1, centerY - 1, 2, 2);
                        g.DrawLine(new Pen(Color.DarkBlue, 1), centerX, centerY, centerX + r, centerY);
                        g.DrawEllipse(new Pen(Color.Red, 1), centerX - r, centerY - r, r + r, r + r);

                        bool flag=false;
                        while (seedNr <= ilosc)
                        {


                            if (reservedIndexes.Count >= setSizeX * setSizeY)
                            {
                                textBox8.Text = "Nie można wyświetlić wszystkich zadanych zarodków ziaren.";
                                break;
                            }



                            flag = false;
                            for(int i = 0; i < setSizeX; i++)
                            {
                                for( int j = 0; j < setSizeY; j++)
                                {
                                    if(reservedIndexes.Contains(grid.currentArray[i,j].index))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        flag = true;

                                        firstCellX = i+1;
                                        firstCellY = j+1;
                                        centerX = (firstCellX * a) - (a / 2);
                                        centerY = (firstCellY * a) - (a / 2);

                                        //g.DrawEllipse(new Pen(Color.Red, 1), centerX - 1, centerY - 1, 2, 2);
                                        //g.DrawLine(new Pen(Color.DarkBlue, 1), centerX, centerY, centerX + r, centerY);
                                        //g.DrawEllipse(new Pen(Color.Red, 1), centerX - r, centerY - r, r + r, r + r);
                                        //flag = false;
                                        for (int m = 0; m < setSizeX; m++)//2 * firstCellX; m++) //indeksy
                                        {
                                            for (int n = 0; n < setSizeY; n++)//2 * firstCellY; n++)
                                            {
                                                //if (m >= setSizeX || n >= setSizeY ) //|| flag==true)
                                                //{
                                                //    break;
                                                //}
                                                if (Math.Pow((m * a - centerX), 2) + Math.Pow(((n + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow(((m + 1) * a - centerX), 2) + Math.Pow(((n + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow((m * a - centerX), 2) + Math.Pow((n * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow(((m + 1) * a - centerX), 2) + Math.Pow((n * a - centerY), 2) <= Math.Pow(r, 2))
                                                {
                                                    if(!reservedIndexes.Contains(grid.currentArray[m, n].index))
                                                    {
                                                        reservedIndexes.Add(grid.currentArray[m, n].index);
                                                    }
                                                    //g.DrawRectangle(new Pen(Color.LightBlue, 2), m * a + 1, n * a + 1, a - 1, a - 1);
                                                    //flag = true;
                                                }
                                            }
                                            //if (flag == true) break;

                                        }

                                        //
                                    }
                                    if (flag == true) break;

                                }
                                if (flag == true) break;

                            }


                            grid.currentArray[firstCellX - 1, firstCellY - 1].currentState = seedNr;
                            g.FillRectangle(new SolidBrush(grainColorTable[seedNr-1]), (firstCellX - 1) * a + 1, (firstCellY - 1) * a + 1, a - 1, a - 1);

                            seedNr++;
                            g.DrawEllipse(new Pen(Color.Red, 1), centerX - 1, centerY - 1, 2, 2);
                            g.DrawLine(new Pen(Color.DarkBlue, 1), centerX, centerY, centerX + r, centerY);
                            g.DrawEllipse(new Pen(Color.Red, 1), centerX - r, centerY - r, r + r, r + r);
                           
                        }


                        break;
                    }
                case 3:
                    {
                        if (ilosc > setSizeX * setSizeY)
                        {
                            textBox8.Text = "Nie można wyświetlić wszystkich zadanych zarodków ziaren.";
                            numberOfGrains = setSizeX * setSizeY;
                            setColorTable(numberOfGrains);

                            int index = 0;
                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {
                                    grid.currentArray[i, j].currentState = index;
                                    g.FillRectangle(new SolidBrush(grainColorTable[index]), i * a + 1, j * a + 1, a - 1, a - 1);

                                    index++;

                                }

                            }
                        }
                        else
                        {
                            numberOfGrains = ilosc;
                            setColorTable(numberOfGrains);

                            int[] tab = new int[ilosc];
                            int rndIndex = 0;

                            void fun(int ii)
                            {
                                rndIndex = rnd.Next(setSizeX * setSizeY);
                                if (tab.Contains(rndIndex))
                                {
                                    fun(ii);
                                }
                                else
                                {
                                    tab[ii] = rndIndex;
                                }
                            }


                            for (int i = 0; i < ilosc; i++)
                            {
                                fun(i);
                            }

                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {
                                    for (int k = 0; k < ilosc; k++)
                                    {
                                        if (grid.currentArray[i, j].index == tab[k])
                                        {
                                            grid.currentArray[i, j].currentState = k;
                                            g.FillRectangle(new SolidBrush(grainColorTable[k]), i * a + 1, j * a + 1, a - 1, a - 1);

                                        }
                                    }
                                }
                            }

                        }



                        break;
                    }
                case 4:
                    {

                        break;
                    }

            }
        }

        public void setColorTable(int index)
        {
            grainColorTable = new Color[index];
            for (int i = 0; i < index; i++)
            {
                grainColorTable[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
        }


        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
