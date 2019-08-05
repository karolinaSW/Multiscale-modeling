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
    public partial class Form6 : Form
    {
        /*
        public static int setSizeX = Form5.setSizeX;
        public static int setSizeY = Form5.setSizeY;
        public static float a = Form5.a; // size of square cell
        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        Bitmap bm;
        public static int numberOfNeighbours = Form5.numberOfNeighbours;
        public static bool flagDynamicNeighbour = Form5.flagDynamicNeighbour;
        public static float radius = Form5.radius;
        public static bool periodic = Form5.periodic;
        public static int nukleationTag = Form5.nukleationTag;
        public static int ilosc_x = Form5.ilosc_x, ilosc_y = Form5.ilosc_y, ilosc = Form5.ilosc, r = Form5.r;
        public static int numberOfGrains = Form5.numberOfGrains;
        public static Color[] grainColorTable = Form5.grainColorTable;
        private Random rnd = new Random();
        public static GridGrains grid = Form5.grid;
        */


        public static int setSizeX;
        public static int setSizeY;
        public static float a; // size of square cell
        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        Bitmap bm;
        public static int numberOfNeighbours;
        public static bool flagDynamicNeighbour;
        public static float radius;
        public static bool periodic;
        public static int nukleationTag;
        public static int ilosc_x, ilosc_y, ilosc, r;
        public static int numberOfGrains;
        public static Color[] grainColorTable;
        private Random rnd = new Random();
        private Random rnd1 = new Random();
        public static GridGrains grid;


        public static double kt;
        public static int liczbaIteracji;
        public static List<int> indeksyLosowanieMC = new List<int>();
        public static int prevEnergia, nextEnergia, deltaEnergia;
        public static Color[] energyColor;

        private void button5_Click(object sender, EventArgs e) // draw energy
        {
            energyColor = new Color[numberOfNeighbours + 1];
            /*
            for (int a = 0; a < 9; a++)
            {
                energyColor[a] = Color.FromArgb(0, 0, a*31); // index w gore -> energia w gore
            }
            */
            if (numberOfNeighbours == 8)
            {
                energyColor[0] = Color.FromArgb(0, 255, 0); // index w gore -> energia w gore zielony
                energyColor[1] = Color.FromArgb(64, 255, 0); // index w gore -> energia w gore
                energyColor[2] = Color.FromArgb(128, 255, 0); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(191, 255, 0); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 255, 0); // index w gore -> energia w gore
                energyColor[5] = Color.FromArgb(255, 191, 0); // index w gore -> energia w gore
                energyColor[6] = Color.FromArgb(255, 128, 0); // index w gore -> energia w gore
                energyColor[7] = Color.FromArgb(255, 64, 0); // index w gore -> energia w gore
                energyColor[8] = Color.FromArgb(255, 0, 0); // index w gore -> energia w gore czerwony
            }
            else if (numberOfNeighbours == 4)
            {
                energyColor[0] = Color.FromArgb(0, 255, 0); // index w gore -> energia w gore zielony
                energyColor[1] = Color.FromArgb(191, 255, 0); // index w gore -> energia w gore
                energyColor[2] = Color.FromArgb(255, 191, 0); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(255, 0, 0); // index w gore -> energia w gore czerwony
            }
            else if (numberOfNeighbours == 5)
            {
                energyColor[0] = Color.FromArgb(0, 255, 0); // index w gore -> energia w gore zielony
                energyColor[2] = Color.FromArgb(128, 255, 0); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 255, 0); // index w gore -> energia w gore
                energyColor[6] = Color.FromArgb(255, 128, 0); // index w gore -> energia w gore
                energyColor[8] = Color.FromArgb(255, 0, 0); // index w gore -> energia w gore czerwony
            }
            else if (numberOfNeighbours == 6)
            {
                energyColor[0] = Color.FromArgb(0, 255, 0); // index w gore -> energia w gore zielony
                energyColor[2] = Color.FromArgb(128, 255, 0); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(191, 255, 0); // index w gore -> energia w gore
                energyColor[5] = Color.FromArgb(255, 191, 0); // index w gore -> energia w gore
                energyColor[6] = Color.FromArgb(255, 128, 0); // index w gore -> energia w gore
                energyColor[8] = Color.FromArgb(255, 0, 0); // index w gore -> energia w gore czerwony
            }

            drawEnergy();

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.DoubleBuffer,
                 true);

            //g.Clear(Color.White);



            setSizeX = Form5.setSizeX;
            setSizeY = Form5.setSizeY;
            a = Form5.a; // size of square cell
            numberOfNeighbours = Form5.numberOfNeighbours;
            flagDynamicNeighbour = Form5.flagDynamicNeighbour;
            radius = Form5.radius;
            periodic = Form5.periodic;
            nukleationTag = Form5.nukleationTag;
            ilosc_x = Form5.ilosc_x;
            ilosc_y = Form5.ilosc_y;
            ilosc = Form5.ilosc;
            r = Form5.r;
            numberOfGrains = Form5.numberOfGrains;
            grainColorTable = Form5.grainColorTable;
            grid = new GridGrains(setSizeX,setSizeY);
            grid = Form5.grid;



            bm = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(bm);

            //DrawGrid(setSizeX, setSizeY, a);
            draw();

        }

        private void button6_Click(object sender, EventArgs e) // clean
        {
            g.Clear(Color.White);
            pictureBox1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form7 frm7 = new Form7();

            frm7.Show();
        }

        private void button4_Click(object sender, EventArgs e) // accept data
        {
            liczbaIteracji = Convert.ToInt32(textBox1.Text);
            kt = Convert.ToDouble(textBox2.Text);

            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    indeksyLosowanieMC.Add(grid.currentArray[i, j].index);
                }
            }
        }

        public Form6()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            grid = new GridGrains(setSizeX, setSizeY);
            grid = Form5.grid;
        }

        private void button2_Click(object sender, EventArgs e) // do montecarlo
        {

            int indxWylosowanejKomorki = 0;
            int indexListyZIndexami = 0;

            for (int iteracja = 0; iteracja < liczbaIteracji; iteracja ++) // iterowanie
            {
                indeksyLosowanieMC.Clear();
                // uzupelnianie na nowo listy
                for (int i = 0; i < setSizeX; i++)
                {
                    for (int j = 0; j < setSizeY; j++)
                    {
                        indeksyLosowanieMC.Add(grid.currentArray[i, j].index);
                    }
                }

                while (indeksyLosowanieMC.Count != 0) // losowanie po siatce
                {
                    //bool flag = true;
                    //while (flag)
                    //{
                        indexListyZIndexami = rnd.Next(indeksyLosowanieMC.Count);
                        indxWylosowanejKomorki = indeksyLosowanieMC[indexListyZIndexami];
                        //if(indeksyLosowanieMC.Contains(indxWylosowanejKomorki))
                        //{
                        //    flag = false;
                        //}
                    //}
                    
                    indeksyLosowanieMC.Remove(indxWylosowanejKomorki); // wylosowano z pozostalych do wylosowania i usuwam z puli
                    bool znaleziono = false;
                    //szukam kom. o danym indeksie
                    for (int i = 0; i < setSizeX; i++)
                    {
                        for (int j = 0; j < setSizeY; j++)
                        {
                            if (grid.currentArray[i,j].index == indxWylosowanejKomorki)
                            {
                                znaleziono = true;

                                prevEnergia = 0;
                                nextEnergia = 0;
                                int indxSasiada = 0;
                                for(int neig = 0; neig < numberOfNeighbours; neig++)
                                {
                                    if(grid.currentArray[i,j].currentState != grid.currentArray[i,j].currentStateOfNeighbours[neig] && grid.currentArray[i, j].currentStateOfNeighbours[neig] != 0)
                                    {
                                        prevEnergia++;
                                    }
                                }

                                grid.currentArray[i, j].energiaKomorki = prevEnergia;

                                int zero = 0;
                                while(zero == 0)
                                {
                                    indxSasiada = rnd.Next(grid.currentArray[i, j].currentStateOfNeighbours.Count());
                                    zero = grid.currentArray[i, j].currentStateOfNeighbours[indxSasiada];
                                }
                                //indxSasiada = rnd.Next(grid.currentArray[i, j].currentStateOfNeighbours.Count());
                                int kopiaStanuKomorki = grid.currentArray[i, j].currentState;
                                grid.currentArray[i, j].currentState = grid.currentArray[i, j].currentStateOfNeighbours[indxSasiada]; // !!!!!!
                                //int newStateRandXIndx = rnd.Next(setSizeX);
                                //int newStateRandYIndx = rnd.Next(setSizeY);
                                //grid.currentArray[i, j].currentState = grid.currentArray[newStateRandXIndx, newStateRandYIndx].currentState; // !!!!!! alternatywnie...


                                for (int neig = 0; neig < numberOfNeighbours; neig++)
                                {
                                    if (grid.currentArray[i, j].currentState != grid.currentArray[i, j].currentStateOfNeighbours[neig] && grid.currentArray[i, j].currentStateOfNeighbours[neig]!= 0)
                                    {
                                        nextEnergia++;
                                    }
                                }

                                grid.currentArray[i, j].energiaKomorki = nextEnergia;


                                deltaEnergia = nextEnergia - prevEnergia;
                                double prawdopodobienstwo;

                                if(deltaEnergia <= 0)
                                {
                                    prawdopodobienstwo = 1;
                                }
                                else
                                {
                                    prawdopodobienstwo = Math.Pow(Math.E, (-(deltaEnergia / kt)));
                                }

                                double los = rnd1.NextDouble();
                                if(los > prawdopodobienstwo)
                                {
                                    //wrocic do wartosci 
                                    grid.currentArray[i, j].currentState = kopiaStanuKomorki;
                                    grid.currentArray[i, j].energiaKomorki = prevEnergia;


                                }




                            }
                            if (znaleziono == true) break;
                        }
                        if (znaleziono == true) break;

                    }

                }

                

            }
            
            g.Clear(Color.White);
            pictureBox1.Refresh();
            draw();
            
        }
        /*
        public void DrawGrid(int setSizeX, int setSizeY, float a)
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



        }
        */

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

        public void drawEnergy()
        {
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j ++)
                {
                    int e = 0;
                    for (int s = 0; s < numberOfNeighbours; s++)
                    {
                        if (grid.currentArray[i, j].currentState != grid.currentArray[i, j].currentStateOfNeighbours[s] && grid.currentArray[i, j].currentStateOfNeighbours[s] != 0)
                        {
                            e++;
                        }
                    }
                    grid.currentArray[i, j].energiaKomorki = e;
                }
            }

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
                    g.FillRectangle(new SolidBrush(energyColor[grid.currentArray[i, j].energiaKomorki]), i * a + 1, j * a + 1, a - 1, a - 1);
                }
            }

        }


    }
}
