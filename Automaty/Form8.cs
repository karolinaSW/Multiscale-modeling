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
    public partial class Form8 : Form
    {
        
        public static int setSizeX;
        public static int setSizeY;
        public static GridGrains grid;
        public static int numberOfNeighbours;
        private System.Drawing.Graphics g;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        public static float a; // size of square cell
        public Bitmap bm;
        public static Color[] energyColor;




        /*
        
        public static bool flagDynamicNeighbour;
        public static float radius;
        public static bool periodic;
        public static int nukleationTag;
        public static int ilosc_x, ilosc_y, ilosc, r;
        public static int numberOfGrains;
        public static Color[] grainColorTable;
        public static List<Color> recrystalizedColorTable = new List<Color>();
        private Random rnd = new Random();
        private Random rnd1 = new Random();
        public static double ro;
        public static double deltaRo;   // dislocation pool
        public static double sredniaPaczka;
        public static double roDlaKazdej;
        //public static int liczbaIteracji;
        public static double A;
        public static double B;
        public static double procent = 0.3;
        public static List<double> listaRo = new List<double>();
        public static double roKrytyczne;
        public static List<int> listaZarodkow = new List<int>();
        public int stateZarodka;
        public static double czas;
        public static double krokCzasowy;
        public static double mnoznik;
        public static double maxDislocation;
        */


        public Form8()
        {
            InitializeComponent();
            setSizeX = Form7.setSizeX;
            setSizeY = Form7.setSizeY;
            //grid = new GridGrains(setSizeX, setSizeY);
            grid = Form7.grid;
            numberOfNeighbours = Form7.numberOfNeighbours;
            g = pictureBox1.CreateGraphics();
            a = Form7.a; // size of square cell
            bm = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            energyColor = new Color[numberOfNeighbours + 1];


            /*
            
            numberOfNeighbours = Form7.numberOfNeighbours;
            grid = Form7.grid;
            g = Graphics.FromImage(bm);
            */
        }

        private void Form8_Load(object sender, EventArgs e)
        {
        /*
            setSizeX = Form6.setSizeX;
            setSizeY = Form6.setSizeY;
            a = Form6.a; // size of square cell
            numberOfNeighbours = Form6.numberOfNeighbours;
            flagDynamicNeighbour = Form6.flagDynamicNeighbour;
            radius = Form6.radius;
            periodic = Form6.periodic;
            nukleationTag = Form6.nukleationTag;
            ilosc_x = Form6.ilosc_x;
            ilosc_y = Form6.ilosc_y;
            ilosc = Form6.ilosc;
            r = Form6.r;
            numberOfGrains = Form6.numberOfGrains;
            grainColorTable = Form7.grainColorTable;
            grid = new GridGrains(setSizeX, setSizeY);
            grid = Form6.grid;
            */

            /*for( int i = 0; i < setSizeX; i ++)
            {
                for( int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j].currentState = Form6.grid.currentArray[i, j].currentState;
                }
            }*/

            bm = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            g = Graphics.FromImage(bm);

            //DrawGrid(setSizeX, setSizeY, a);
            //draw();




            //DrawGrid(setSizeX, setSizeY, a);
            //draw();
            //energyColor = new Color[numberOfNeighbours + 1];

            if (numberOfNeighbours == 8)
            {
                energyColor[0] = Color.FromArgb(131, 0, 255); // index w gore -> energia w gore fiol
                energyColor[1] = Color.FromArgb(26, 75, 221); // index w gore -> energia w gore nieb
                energyColor[2] = Color.FromArgb(0, 229, 255); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(21, 234, 31); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 250, 0); // index w gore -> energia w gore
                energyColor[5] = Color.FromArgb(255, 144, 0); // index w gore -> energia w gore
                energyColor[6] = Color.FromArgb(255, 12, 0); // index w gore -> energia w gore
                energyColor[7] = Color.FromArgb(127, 0, 0); // index w gore -> energia w gore
                energyColor[8] = Color.FromArgb(53, 0, 0); // index w gore -> energia w gore czerwony
            }
            else if (numberOfNeighbours == 4)
            {
                energyColor[0] = Color.FromArgb(26, 75, 221); // index w gore -> energia w gore nieb
                energyColor[1] = Color.FromArgb(21, 234, 31); // index w gore -> energia w gore
                energyColor[2] = Color.FromArgb(255, 250, 0); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(255, 144, 0); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 12, 0); // index w gore -> energia w gore

            }
            else if (numberOfNeighbours == 5)
            {
                energyColor[0] = Color.FromArgb(26, 75, 221); // index w gore -> energia w gore nieb
                energyColor[1] = Color.FromArgb(0, 229, 255); // index w gore -> energia w gore
                energyColor[2] = Color.FromArgb(21, 234, 31); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(255, 250, 0); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 144, 0); // index w gore -> energia w gore
                energyColor[5] = Color.FromArgb(255, 12, 0); // index w gore -> energia w gore

            }
            else if (numberOfNeighbours == 6)
            {
                energyColor[0] = Color.FromArgb(26, 75, 221); // index w gore -> energia w gore nieb
                energyColor[1] = Color.FromArgb(0, 229, 255); // index w gore -> energia w gore
                energyColor[2] = Color.FromArgb(21, 234, 31); // index w gore -> energia w gore
                energyColor[3] = Color.FromArgb(255, 250, 0); // index w gore -> energia w gore
                energyColor[4] = Color.FromArgb(255, 144, 0); // index w gore -> energia w gore
                energyColor[5] = Color.FromArgb(255, 12, 0); // index w gore -> energia w gore
                energyColor[6] = Color.FromArgb(127, 0, 0); // index w gore -> energia w gore

            }

            drawEnergy();
        }
        public void drawEnergy()
        {
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    int e = 0;
                    if (grid.currentArray[i, j].drxState == -1)
                    {
                        for (int s = 0; s < numberOfNeighbours; s++)
                        {
                            if (grid.currentArray[i, j].neighbour[s].drxState == -1)
                            {
                                if (grid.currentArray[i, j].currentState != grid.currentArray[i, j].neighbour[s].currentState && grid.currentArray[i, j].neighbour[s].currentState != 0)
                                {
                                    e++;
                                }
                            }
                            else
                            {
                                e++;
                            }
                        }
                        grid.currentArray[i, j].energiaKomorki = e;
                    }
                    else
                    {
                        for (int s = 0; s < numberOfNeighbours; s++)
                        {
                            if (grid.currentArray[i, j].neighbour[s].drxState == -1)
                            {
                                e++;
                            }
                            else
                            {
                                if (grid.currentArray[i, j].drxState != grid.currentArray[i, j].neighbour[s].drxState)
                                {
                                    e++;
                                }
                            }
                        }
                        grid.currentArray[i, j].energiaKomorki = e;
                    }
                    /*
                    for (int s = 0; s < numberOfNeighbours; s++)
                    {
                        
                        if (grid.currentArray[i, j].currentState != grid.currentArray[i, j].currentStateOfNeighbours[s] && grid.currentArray[i, j].currentStateOfNeighbours[s] != 0)
                        {
                            e++;
                        }
                    }
                    grid.currentArray[i, j].energiaKomorki = e;
                    */
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
/*
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
        */

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}