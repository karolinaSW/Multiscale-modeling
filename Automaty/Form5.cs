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
        Random rand = new Random();
        int modHex;
        int modPent;
        //int click = 0;

        public static int setSizeX = 0;
        public static int setSizeY = 0;
        public static float a; // size of square cell
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
        public static Color[] grainColorTable;
        private Random rnd = new Random();
        public static GridGrains grid;
        public static GridGrains gridForMC = new GridGrains(setSizeX, setSizeY);





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
            //click = 0;
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
                numberOfNeighbours = 100;
                radius = Convert.ToInt32(textBox11.Text);
            }



            grid = new GridGrains(setSizeX, setSizeY);
            int k = 0;
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j] = new CellGrains(numberOfNeighbours);

                    grid.currentArray[i, j].index = k;
                    k++;
                }
            }
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j].insidePointX = (rnd.Next(Convert.ToInt32((i * a) * 1000), Convert.ToInt32(((i + 1) * a) * 1000))) / 1000f;
                    grid.currentArray[i, j].insidePointY = (rnd.Next(Convert.ToInt32((j * a) * 1000), Convert.ToInt32(((j + 1) * a) * 1000))) / 1000f;
                    
                }
            }


            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    for(int s = 0; s < numberOfNeighbours; s++)
                    {
                        //grid.currentArray[i, j].currentNeighbours[s] = new CellGrains(numberOfNeighbours);
                        //grid.currentArray[i,j].currentNeighbours[s] = new CellGrains(numberOfNeighbours);
                        grid.currentArray[i, j].neighbour[s] = new CellGrains(numberOfNeighbours);

                    }
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

        private void button5_Click(object sender, EventArgs e) // next step
        {

            neighbourhoodInit();
            //click++;

            computeNewStates();
            proceed();
            draw();
            //proceed(); draw();
            pictureBox1.Refresh();
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
                modPent = rand.Next(4);
            }
            if (radioButton4.Checked == true)
            {
                numberOfNeighbours = 6;
                modHex = rand.Next(2);

            }
            if (radioButton5.Checked == true && textBox11 != null)
            {
                flagDynamicNeighbour = true;
                numberOfNeighbours = 100;
                radius = Convert.ToInt32(textBox11.Text);
            }
            if (checkBox1.CheckState == CheckState.Checked)
            {
                periodic = true;
            }
            else
            {
                periodic = false;
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


                        int index = 1;
                        for (int i = 0; i < setSizeX; i = i + odstX + 1)
                        {
                            int indY = 0;
                            for (int j = 0; j < setSizeY; j = j + odstY + 1)
                            {
                                if (indY >= ilosc_y)
                                {
                                    break;
                                }
                                if (index > ilosc_x * ilosc_y)
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
                                    if (!reservedIndexes.Contains(grid.currentArray[i, j].index))
                                    {
                                        reservedIndexes.Add(grid.currentArray[i, j].index);

                                    }
                                    g.FillRectangle(new SolidBrush(Color.LightCoral), i * a + 1, j * a + 1, a - 1, a - 1);

                                }

                                //g.DrawEllipse(new Pen(Color.Red, 1), grid.currentArray[i, j].insidePointX - 1, grid.currentArray[i, j].insidePointX - 1, 2, 2);


                            }
                        }
                        grid.currentArray[firstCellX - 1, firstCellY - 1].currentState = seedNr;
                        g.FillRectangle(new SolidBrush(grainColorTable[seedNr]), (firstCellX - 1) * a + 1, (firstCellY - 1) * a + 1, a - 1, a - 1);

                        seedNr++;
                        g.DrawEllipse(new Pen(Color.Red, 1), centerX - 1, centerY - 1, 2, 2);
                        g.DrawLine(new Pen(Color.DarkBlue, 1), centerX, centerY, centerX + r, centerY);
                        g.DrawEllipse(new Pen(Color.Red, 1), centerX - r, centerY - r, r + r, r + r);



                        bool flag = false;
                        while (seedNr <= ilosc)
                        {
                            if (reservedIndexes.Count >= setSizeX * setSizeY)
                            {
                                textBox8.Text = "Nie można wyświetlić wszystkich zadanych zarodków ziaren.";
                                break;
                            }

                            flag = false;
                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {
                                    if (reservedIndexes.Contains(grid.currentArray[i, j].index))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        flag = true;

                                        firstCellX = i + 1;
                                        firstCellY = j + 1;
                                        centerX = (firstCellX * a) - (a / 2);
                                        centerY = (firstCellY * a) - (a / 2);

                                        for (int m = 0; m < setSizeX; m++)
                                        {
                                            for (int n = 0; n < setSizeY; n++)
                                            {
                                                if (Math.Pow((m * a - centerX), 2) + Math.Pow(((n + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow(((m + 1) * a - centerX), 2) + Math.Pow(((n + 1) * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow((m * a - centerX), 2) + Math.Pow((n * a - centerY), 2) <= Math.Pow(r, 2)
                                                    || Math.Pow(((m + 1) * a - centerX), 2) + Math.Pow((n * a - centerY), 2) <= Math.Pow(r, 2))
                                                {
                                                    if (!reservedIndexes.Contains(grid.currentArray[m, n].index))
                                                    {
                                                        reservedIndexes.Add(grid.currentArray[m, n].index);
                                                    }

                                                }
                                            }

                                        }

                                    }
                                    if (flag == true) break;

                                }
                                if (flag == true) break;

                            }

                            grid.currentArray[firstCellX - 1, firstCellY - 1].currentState = seedNr;
                            g.FillRectangle(new SolidBrush(grainColorTable[seedNr]), (firstCellX - 1) * a + 1, (firstCellY - 1) * a + 1, a - 1, a - 1);

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

                            int seedNr = 1;
                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {
                                    grid.currentArray[i, j].currentState = seedNr;
                                    g.FillRectangle(new SolidBrush(grainColorTable[seedNr]), i * a + 1, j * a + 1, a - 1, a - 1);

                                    seedNr++;

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
                                            grid.currentArray[i, j].currentState = k + 1;
                                            g.FillRectangle(new SolidBrush(grainColorTable[k + 1]), i * a + 1, j * a + 1, a - 1, a - 1);

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
            grainColorTable = new Color[index + 1];
            for (int i = 0; i < index + 1; i++)
            {
                grainColorTable[i] = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            }
        }


        public void neighbourhoodInit()
        {
            if (periodic == true)
            {
                switch (numberOfNeighbours)
                {
                    case 4:
                        {
                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {

                                    if (i == 0 && j == 0) //lg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;

                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];


                                    }
                                    else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                    {
                                        
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, 0];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, setSizeY - 1];

                                    }
                                    else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY-1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }
                                    else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, setSizeY-1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }
                                    else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                    {

                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX-1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];

                                    }
                                    else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                    {

                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;

                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }

                                    else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY-1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }
                                    else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, 0];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }
                                    else // srodek siatki, nie na brzegu
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                    }

                                }
                            }
                                break;
                        }

                    case 8:
                        {

                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {

                                    if (i == 0 && j == 0) //lg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[setSizeX - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;

                                        //..................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[setSizeX - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[setSizeX - 1, setSizeY - 1];


                                    }
                                    else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                    {
                                       
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[setSizeX - 1, j - 1].currentState;

                                        //...................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, 0];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, 0];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[setSizeX - 1, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[setSizeX - 1, j - 1];

                                    }
                                    else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, setSizeY - 1].currentState;

                                        // ..........................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, setSizeY - 1];
                                    }
                                    else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[0, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;

                                        //..................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[0, 0];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, 0];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];
                                    }
                                    else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[setSizeX - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[setSizeX - 1, j - 1].currentState;

                                        //...............................
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[setSizeX - 1, j];
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[setSizeX - 1, j - 1];

                                    }
                                    else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[0, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;

                                        //................................
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[0, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];
                                    }

                                    else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, setSizeY - 1].currentState;

                                        //...................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, setSizeY - 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, setSizeY - 1];
                                    }
                                    else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, 0].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;

                                        //..............
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, 0];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, 0];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];

                                    }
                                    else // srodek siatki, nie na brzegu
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;

                                        //.............................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];
                                    }

                                }
                            }

                            break;
                        }

                    case 5:
                        {
                            switch (modPent)
                            {
                                case 0: // u //do gory
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j];

                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, 0].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, 0];
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j];

                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[0, j].currentState; ;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;

                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 1: // n // do dolu
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, setSizeY - 1];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j - 1];

                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j - 1];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState; ;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState; ;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 2: // c // prawo
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState; 
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[setSizeX - 1, j + 1].currentState; 
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState; 
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1]; 
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[setSizeX - 1, j + 1]; 
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j]; 
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, setSizeY - 1];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[setSizeX - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[setSizeX - 1, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j - 1];
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[setSizeX - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j - 1];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 3: // rozrost do lewej
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik 
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[0, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[0, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState; ;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, 0].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, 0];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                            }
                            break;
                        }

                    case 6:
                        {
                            
                            /*if (click == 0)
                            { 
                                mod = rand.Next(2);
                            }*/
                            switch (modHex)
                            {
                                case 0: //done
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, 0];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, 0];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }

                                            }
                                        }
                                        break;
                                    }

                                case 1: //done
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, setSizeY - 1];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {
                                                    
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j-1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j - 1];
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[setSizeX - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[setSizeX - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[setSizeX - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[setSizeX - 1, j - 1];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[0, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[0, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[0, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[0, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, setSizeY - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i -1, setSizeY - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, setSizeY - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, setSizeY - 1];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, 0].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, 0];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, 0];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }

                    case 100:
                        {
                            // rand uklad
                            break;
                        }
                }








            }
            else // nie periodyczne
            {

                switch (numberOfNeighbours)
                {
                    case 4:
                        {

                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {

                                    if (i == 0 && j == 0) //lg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();

                                    }
                                    else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;

                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        
                                    }
                                    else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }
                                    else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }
                                    else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                    {
                                        
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();


                                    }
                                    else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }

                                    else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }
                                    else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }
                                    else // srodek siatki, nie na brzegu
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];

                                    }

                                }
                            }
                            break;
                        }
// TUTUTUTUTUTUTUTUTUTUTUTUUTUTTU

                    case 8:
                        {

                            for (int i = 0; i < setSizeX; i++)
                            {
                                for (int j = 0; j < setSizeY; j++)
                                {
                                    
                                    if (i == 0 && j == 0) //lg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[6] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[7] = new CellGrainsBase();
                                    }
                                    else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                    {
                                        
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[6] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[7] = new CellGrainsBase();
                                    }
                                    else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = new CellGrainsBase();
                                    }
                                    else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];

                                    }
                                    else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[6] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[7] = new CellGrainsBase();

                                    }
                                    else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];

                                    }

                                    else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = 0;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = new CellGrainsBase();

                                    }
                                    else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                    {
                                        
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];

                                    }
                                    else // srodek siatki, nie na brzegu
                                    {
                                        grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j + 1].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[6] = grid.currentArray[i - 1, j].currentState;
                                        grid.currentArray[i, j].currentStateOfNeighbours[7] = grid.currentArray[i - 1, j - 1].currentState;
                                        //............................................................
                                        grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                        grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                        grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                        grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                        grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                        grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j + 1];
                                        grid.currentArray[i, j].neighbour[6] = grid.currentArray[i - 1, j];
                                        grid.currentArray[i, j].neighbour[7] = grid.currentArray[i - 1, j - 1];

                                    }

                                }
                            }

                            break;
                        }

                    case 5:
                        {

                            switch(modPent)
                            {
                                case 0: // u
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();

                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 1: // n
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();

                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 2: // c
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j -1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                                case 3:
                                    {

                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i, j + 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i, j + 1];
                                                }

                                            }
                                        }

                                        break;
                                    }
                            }

                            break;
                        }

                    case 6:
                        {
                            
                            switch (modHex)
                            {
                                case 0:
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();

                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j - 1];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j];
                                                }

                                            }
                                        }
                                        break;
                                    }

                                case 1:
                                    {
                                        for (int i = 0; i < setSizeX; i++)
                                        {
                                            for (int j = 0; j < setSizeY; j++)
                                            {

                                                if (i == 0 && j == 0) //lg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();

                                                }
                                                else if (i == 0 && j == setSizeY - 1) //ld naroznik
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == 0) //pg naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                                }
                                                else if (i == setSizeX - 1 && j == setSizeY - 1) //pd naroznik
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else if (i == 0 && j != 0 && j != setSizeY - 1) //lewa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();

                                                }
                                                else if (i == setSizeX - 1 && j != 0 && j != setSizeY - 1) // prawa krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }

                                                else if (j == 0 && i != 0 && i != setSizeX - 1) // gorna krawedz
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = 0;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = new CellGrainsBase();
                                                }
                                                else if (j == setSizeY - 1 && i != 0 && i != setSizeX - 1) // dolna krawedz
                                                {

                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = 0;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[3] = new CellGrainsBase();
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }
                                                else // srodek siatki, nie na brzegu
                                                {
                                                    grid.currentArray[i, j].currentStateOfNeighbours[0] = grid.currentArray[i, j - 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[1] = grid.currentArray[i + 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[2] = grid.currentArray[i + 1, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[3] = grid.currentArray[i, j + 1].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[4] = grid.currentArray[i - 1, j].currentState;
                                                    grid.currentArray[i, j].currentStateOfNeighbours[5] = grid.currentArray[i - 1, j - 1].currentState;
                                                    //............................................................
                                                    grid.currentArray[i, j].neighbour[0] = grid.currentArray[i, j - 1];
                                                    grid.currentArray[i, j].neighbour[1] = grid.currentArray[i + 1, j];
                                                    grid.currentArray[i, j].neighbour[2] = grid.currentArray[i + 1, j + 1];
                                                    grid.currentArray[i, j].neighbour[3] = grid.currentArray[i, j + 1];
                                                    grid.currentArray[i, j].neighbour[4] = grid.currentArray[i - 1, j];
                                                    grid.currentArray[i, j].neighbour[5] = grid.currentArray[i - 1, j - 1];
                                                }

                                            }
                                        }
                                        break;
                                    }
                            }

                            

                            break;
                        }

                    case 100:
                        {
                            // rand uklad
                            break;
                        }
                }




            
            }
        } // warunki periodyczne lub nie

        public void computeNewStates()
        {


            int[] tab = new int[numberOfNeighbours];
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    var pairs = new List<KeyValuePair<int, int>>();

                    for (int k = 0; k < numberOfNeighbours; k++)
                    {
                        tab[k] = grid.currentArray[i, j].currentStateOfNeighbours[k];
                    }
                    for (int k = 0; k < numberOfNeighbours; k++)
                    {
                        int wartosc = tab[k];
                        int licznik = 0;

                        for (int l = 0; l < numberOfNeighbours; l++)
                        {
                            if (wartosc == tab[l])
                            {
                                licznik++;

                            }
                        }
                        if (!pairs.Contains(new KeyValuePair<int, int>(wartosc, licznik)) && wartosc != 0)
                        {
                            pairs.Add(new KeyValuePair<int, int>(wartosc, licznik));
                        }
                    }

                    /*
                    int licznikWartosci = 0, wartoscWartosci = 0, licznikWartosciMax = 0;

                    for (int k = 0; k < pairs.Count; k++)
                    {
                        licznikWartosci = 0;
                        if (pairs[k].Key != 0)
                        {
                            wartoscWartosci = pairs[k].Value;
                            for (int l = 0; l < pairs.Count; l++)
                            {
                                if (wartoscWartosci == pairs[l].Value)
                                {
                                    licznikWartosci++;
                                }

                            }
                            if (licznikWartosci > licznikWartosciMax)
                            {
                                licznikWartosciMax = licznikWartosci;
                            }
                        }
                    }*/

                    //int[] maxKlucze = new int[licznikWartosciMax];
                    List<int> maxKlucze = new List<int>() ;

                    int maxV = 0, maxK = 0;
                    for (int p = 0; p < pairs.Count; p++)
                    {
                        if (pairs[p].Key != 0 && pairs[p].Value > maxV)
                        {
                            maxV = pairs[p].Value; // tu mam liczebnosc danego ziarna
                            maxK = pairs[p].Key; // tu mam numer id ziarna
                        }
                    }

                    for (int p = 0; p < pairs.Count; p++)
                    {
                        if (pairs[p].Value == maxV)
                        {
                            maxKlucze.Add(pairs[p].Key);
                        }
                    }


                    if (grid.currentArray[i, j].currentState > 0 ) //&& maxK == 0)
                    {
                        grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;
                        continue;
                    }
                    else if ((grid.currentArray[i, j].currentState == 0 && maxK == 0) || (grid.currentArray[i, j].currentState == 0 && maxK > 0)) // || (grid.currentArray[i, j].currentState > 0 && maxK > 0))
                    {
                        int los;
                        if (maxKlucze.Count == 0)
                        {
                            grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;

                        }
                        else
                        {
                            los = rnd.Next(maxKlucze.Count);
                            grid.newArray[i, j].currentState = maxKlucze[los];

                        }

                    }

                }
            }



            /*
            switch (numberOfNeighbours)
            {
                case 4:
                    {

                        int[] tab = new int[numberOfNeighbours];
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                var pairs = new List<KeyValuePair<int, int>>();

                                for (int k = 0; k < numberOfNeighbours; k++)
                                {
                                    tab[k] = grid.currentArray[i, j].currentStateOfNeighbours[k];
                                }
                                for (int k = 0; k < numberOfNeighbours; k++)
                                {
                                    int wartosc = tab[k];
                                    int licznik = 0;

                                    for (int l = 0; l < numberOfNeighbours; l++)
                                    {
                                        if (wartosc == tab[l])
                                        {
                                            licznik++;
                                        }
                                    }
                                    if (!pairs.Contains(new KeyValuePair<int, int>(wartosc, licznik)) && wartosc != 0)
                                    {
                                        pairs.Add(new KeyValuePair<int, int>(wartosc, licznik));
                                    }
                                }


                                int licznikWartosci = 0, wartoscWartosci = 0, licznikWartosciMax = 0;

                                for (int k = 0; k < pairs.Count; k++)
                                {
                                    licznikWartosci = 0;
                                    if (pairs[k].Key != 0)
                                    {
                                        wartoscWartosci = pairs[k].Value;
                                        for (int l = 0; l < pairs.Count; l++)
                                        {
                                            if (wartoscWartosci == pairs[l].Value)
                                            {
                                                licznikWartosci++;
                                            }

                                        }
                                        if (licznikWartosci > licznikWartosciMax)
                                        {
                                            licznikWartosciMax = licznikWartosci;
                                        }
                                    }
                                }

                                int[] maxKlucze = new int[licznikWartosciMax];

                                int maxV = 0, maxK = 0;
                                for (int p = 0; p < pairs.Count; p++)
                                {
                                    if (pairs[p].Key != 0 && pairs[p].Value > maxV)
                                    {
                                        maxV = pairs[p].Value; // tu mam liczebnosc danego ziarna
                                        maxK = pairs[p].Key; // tu mam numer id ziarna
                                    }
                                }

                                for (int p = 0, r = 0; p < pairs.Count; p++)
                                {
                                    if (pairs[p].Value == maxV)
                                    {
                                        maxKlucze[r] = pairs[p].Key;
                                        r++;
                                    }
                                }


                                if (grid.currentArray[i, j].currentState > 0 && maxK == 0)
                                {
                                    grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;
                                    continue;
                                }
                                else if ((grid.currentArray[i, j].currentState == 0 && maxK == 0) || (grid.currentArray[i, j].currentState == 0 && maxK > 0) || (grid.currentArray[i, j].currentState > 0 && maxK > 0))
                                {
                                    int los;
                                    if (licznikWartosciMax == 0)
                                    {
                                        grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;

                                    }
                                    else
                                    {
                                        los = rnd.Next(licznikWartosciMax);
                                        grid.newArray[i, j].currentState = maxKlucze[los];

                                    }

                                }

                            }
                        }

                        break;
                    }
                case 8:
                    {

                        int[] tab = new int[numberOfNeighbours];
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                var pairs = new List<KeyValuePair<int, int>>();

                                for (int k = 0; k < numberOfNeighbours; k++)
                                {
                                    tab[k] = grid.currentArray[i, j].currentStateOfNeighbours[k];
                                }
                                for (int k = 0; k < numberOfNeighbours; k++)
                                {
                                    int wartosc = tab[k];
                                    int licznik = 0;

                                    for (int l = 0; l < numberOfNeighbours; l++)
                                    {
                                        if (wartosc == tab[l])
                                        {
                                            licznik++;
                                        }
                                    }
                                    if (!pairs.Contains(new KeyValuePair<int, int>(wartosc, licznik)) && wartosc!=0)
                                    {
                                        pairs.Add(new KeyValuePair<int, int>(wartosc, licznik));
                                    }
                                }
                                

                                int licznikWartosci = 0, wartoscWartosci = 0, licznikWartosciMax = 0;

                                for (int k = 0; k < pairs.Count; k++)
                                {
                                    licznikWartosci = 0;
                                    if (pairs[k].Key != 0)
                                    {
                                        wartoscWartosci = pairs[k].Value;
                                        for (int l = 0; l < pairs.Count; l++)
                                        {
                                            if (wartoscWartosci == pairs[l].Value)
                                            {
                                                licznikWartosci++;
                                            }

                                        }
                                        if (licznikWartosci > licznikWartosciMax)
                                        {
                                            licznikWartosciMax = licznikWartosci;
                                        }
                                    }
                                }

                                int[] maxKlucze = new int[licznikWartosciMax];

                                int maxV = 0, maxK = 0;
                                for (int p = 0; p < pairs.Count; p++)
                                {
                                    if (pairs[p].Key != 0 && pairs[p].Value > maxV)
                                    {
                                        maxV = pairs[p].Value; // tu mam liczebnosc danego ziarna
                                        maxK = pairs[p].Key; // tu mam numer id ziarna
                                    }
                                }

                                for (int p = 0, r = 0; p < pairs.Count; p++)
                                {
                                    if (pairs[p].Value == maxV)
                                    {
                                        maxKlucze[r] = pairs[p].Key;
                                        r++;
                                    }
                                }


                                if (grid.currentArray[i, j].currentState > 0 && maxK == 0)
                                {
                                    grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;
                                    continue;
                                }
                                else if ((grid.currentArray[i, j].currentState == 0 && maxK == 0) || (grid.currentArray[i, j].currentState == 0 && maxK > 0) || (grid.currentArray[i, j].currentState > 0 && maxK > 0))
                                {
                                    int los;
                                    if (licznikWartosciMax == 0)
                                    {
                                        grid.newArray[i, j].currentState = grid.currentArray[i, j].currentState;

                                    }
                                    else
                                    {
                                        los = rnd.Next(licznikWartosciMax);
                                        grid.newArray[i, j].currentState = maxKlucze[los];

                                    }

                                }

                            }
                        }


                        break;
                    }
                case 5:
                    {
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 100:
                    {
                        break;
                    }
            }

            */
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // przejscie do MC
        {
            
            gridForMC = grid;
            Form6 frm6 = new Form6();

            frm6.Show();
        }

        public void proceed()
        {
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j].currentState = grid.newArray[i, j].currentState;
                    //grid.newArray[i, j].currentState = 0;

                }
            }
        }

        public void draw()
        {
            g.Clear(Color.White);
            pictureBox1.Refresh();


            for (int i = 0; i <= setSizeX; i++)
            {
                g.DrawLine(pen_line, i * a, 0, i * a, setSizeY * a);

            }

            for (int i = 0; i <= setSizeY; i++)
            {
                g.DrawLine(pen_line, 0, i * a, setSizeX * a, i * a);

            }
            pictureBox1.Image = bm;

            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    if (grid.currentArray[i, j].currentState != 0)
                    {
                        g.FillRectangle(new SolidBrush(grainColorTable[grid.currentArray[i, j].currentState]), i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
