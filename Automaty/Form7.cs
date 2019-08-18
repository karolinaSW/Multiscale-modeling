using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Automaty
{
    public partial class Form7 : Form
    {


        public static int setSizeX;
        public static int setSizeY;
        public static float a; // size of square cell
        private System.Drawing.Graphics g;
        private System.Drawing.Graphics graf;
        private System.Drawing.Pen pen_line = new System.Drawing.Pen(Color.Black, 1);
        public Bitmap bm;
        public static int numberOfNeighbours;
        public static bool flagDynamicNeighbour;
        public static float radius;
        public static bool periodic;
        public static int nukleationTag;
        public static int ilosc_x, ilosc_y, ilosc, r;
        public static int numberOfGrains;
        public static Color[] grainColorTable;
        public static List <Color> recrystalizedColorTable = new List<Color>();
        private Random rnd = new Random();
        private Random rnd1 = new Random();
        public static GridGrains grid;
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
        public static Color[] energyColor;






        private void button1_Click(object sender, EventArgs e) // start
        {
            
            label7.Text = "Proces wystartował...";
            
            for(int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    grid.currentArray[i, j].kolor = grainColorTable[grid.currentArray[i, j].currentState];

                }
            }
            

            //stateZarodka = 0;
            stateZarodka = 1;
            recrystalizedColorTable.Add(Color.White);

            A = Convert.ToDouble(textBox2.Text);
            B = Convert.ToDouble(textBox3.Text);
            //liczbaIteracji = Convert.ToInt32(textBox1.Text);
            czas = Convert.ToDouble(textBox5.Text);
            krokCzasowy = Convert.ToDouble(textBox1.Text);
            mnoznik = 1 / krokCzasowy;
            roKrytyczne = Convert.ToDouble(textBox4.Text);

            List<Point> recrystalizedCellOld = new List<Point>();
            List<Point> recrystalizedCellNew = new List<Point>();
            listaRo.Add(0);

            double timestep = 0; // kolejna podzialka czasowa 
            label10.Text = Convert.ToString(Math.Round(maxDislocation,2));
            label10.Refresh();


            for (int iteracja = 0; iteracja < Convert.ToInt32(czas * mnoznik); iteracja += Convert.ToInt32(krokCzasowy * mnoznik))
            {
                label3.Text = Convert.ToString(Math.Round(timestep,3));
                label3.Refresh();

                label10.Text = Convert.ToString(Math.Round(maxDislocation, 2));
                if(Convert.ToDouble(label10.Text) > 4215840142323.42)
                {
                    label10.ForeColor = Color.Green;
                }
                label10.Refresh();

                ComputeEnergy();

                ro = (A / B) + ((1 - (A / B)) * Math.Exp(-B * timestep));
                timestep += krokCzasowy;
                deltaRo = ro - listaRo[iteracja];
                sredniaPaczka = deltaRo / (setSizeX*setSizeY);
                roDlaKazdej = sredniaPaczka * procent;

                for(int i = 0; i < setSizeX; i++)
                {
                    for(int j = 0; j < setSizeY; j ++)
                    {
                        grid.currentArray[i, j].dislocationDensity += roDlaKazdej;
                        deltaRo -= roDlaKazdej;
                        if (grid.currentArray[i, j].dislocationDensity > maxDislocation)
                        {
                            maxDislocation = grid.currentArray[i, j].dislocationDensity;
                        }

                    }
                }

                //roDlaKazdej = deltaRo * ;
                bool IsAssigned;

                while (deltaRo > 0)
                {
                    IsAssigned = false;

                    if (deltaRo - roDlaKazdej < 0)
                    {
                        roDlaKazdej = deltaRo;
                        deltaRo = 0;
                    }

                    double random = rnd.NextDouble();

                    if(random < 0.8)
                    {
                        int licznikGranicznych = 0;
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                if(grid.currentArray[i,j].energiaKomorki > 0)   // na zewnatrz ta petle
                                {
                                    licznikGranicznych++;
                                }
                            }
                        }
                        int indx = rnd.Next(licznikGranicznych);
                        int indx2 = 0;
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                if (grid.currentArray[i, j].energiaKomorki > 0)
                                {
                                    if(indx2 == indx)
                                    {
                                        grid.currentArray[i, j].dislocationDensity += roDlaKazdej;
                                        IsAssigned = true;
                                        if(grid.currentArray[i,j].dislocationDensity > maxDislocation)
                                        {
                                            maxDislocation = grid.currentArray[i, j].dislocationDensity;
                                        }
                                    }
                                    indx2++;
                                }
                            }
                        }
                    }
                    else
                    {
                        int licznikWewnetrznych = 0;
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                if (grid.currentArray[i, j].energiaKomorki == 0)
                                {
                                    licznikWewnetrznych++;
                                }
                            }
                        }
                        int indx = rnd.Next(licznikWewnetrznych);
                        int indx2 = 0;
                        for (int i = 0; i < setSizeX; i++)
                        {
                            for (int j = 0; j < setSizeY; j++)
                            {
                                if (grid.currentArray[i, j].energiaKomorki == 0)
                                {
                                    if (indx2 == indx)
                                    {
                                        grid.currentArray[i, j].dislocationDensity += roDlaKazdej;
                                        IsAssigned = true;
                                        if (grid.currentArray[i, j].dislocationDensity > maxDislocation)
                                        {
                                            maxDislocation = grid.currentArray[i, j].dislocationDensity;
                                        }

                                    }
                                    indx2++;
                                }
                            }
                        }
                    }

                    /*int x = rnd.Next(width);
                    int y = rnd.Next(height);
                    if (oldBoard[x][y].IsNeighbor)
                    {
                        if (rnd.NextDouble() < 0.8)
                        {
                            oldBoard[x][y].densityOfDislocations += roForAll;
                            IsAssigned = true;
                        }
                    }
                    else
                    {
                        if (rnd.NextDouble() < 0.2)
                        {
                            oldBoard[x][y].densityOfDislocations += roForAll;
                            IsAssigned = true;
                        }
                    }*/

                    if (IsAssigned)
                    {
                        deltaRo -= roDlaKazdej;
                    }
                }

                listaRo.Add(ro);
                //czy stworzyc jakas tablice dla zarodkow,poszerzyc number of seeds czy coś .. ?
                //zarodkowanie
                for (int i = 0; i < setSizeX; i++)
                {
                    for (int j = 0; j < setSizeY; j++)
                    {
                        if (grid.currentArray[i, j].energiaKomorki > 0 && grid.currentArray[i,j].dislocationDensity > roKrytyczne)
                        {
                            grid.currentArray[i, j].drxState = stateZarodka;
                            
                            grid.currentArray[i, j].recristalized = true;
                            grid.currentArray[i, j].dislocationDensity = 0;
                            //listaZarodkow.Add(grid.currentArray[i,j].index);
                            recrystalizedColorTable.Add(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                            grid.currentArray[i, j].iterationRecystalized = iteracja;
                            grid.currentArray[i, j].kolor = recrystalizedColorTable[grid.currentArray[i, j].drxState];
                            stateZarodka += 1;
                            //ComputeEnergy();


                        }
                    }
                }
                //rozrost
                bool najwiekszaDyslokacja;
                for (int i = 0; i < setSizeX; i++)
                {
                    for (int j = 0; j < setSizeY; j++)
                    {

                        najwiekszaDyslokacja = true;
                        for (int s = 0; s < numberOfNeighbours; s++)
                        {
                            if(grid.currentArray[i,j].neighbour[s].dislocationDensity > grid.currentArray[i,j].dislocationDensity)
                            {
                                najwiekszaDyslokacja = false;
                                break;
                            }
                        }

                        if (najwiekszaDyslokacja)//(grid.currentArray[i, j].recristalized == false && najwiekszaDyslokacja)
                        {
                            for (int s = 0; s < numberOfNeighbours; s++)
                            {
                                if (grid.currentArray[i, j].neighbour[s].recristalized == true && grid.currentArray[i, j].neighbour[s].iterationRecystalized == (iteracja - 1))
                                {
                                    grid.currentArray[i, j].recristalized = true;
                                    grid.currentArray[i, j].dislocationDensity = 0;
                                    //grid.currentArray[i, j].drxState = grid.currentArray[i, j].currentNeighbours[s].drxState;
                                    grid.currentArray[i, j].drxState = grid.currentArray[i, j].neighbour[s].drxState;

                                    grid.currentArray[i, j].kolor = grid.currentArray[i, j].neighbour[s].kolor;
                                    grid.currentArray[i, j].iterationRecystalized = iteracja;
                                    
                                }
                            }
                        }

                   

                    }
                }
                ComputeEnergy();

                //g.Clear(Color.White);
                //pictureBox1.Refresh();
                drawFromCell();
                pictureBox1.Refresh();

                //Thread.Sleep(500);


                /*
                for (int i = 0; i < setSizeX; i++)
                {
                    for (int j = 0; j < setSizeY; j++)
                    {

                        double wartoscMax = 0;
                        bool flag = true;
                        // przechodzenie po sasiadach, porownanie dislocation

                        for (sasiedzi)
                        {
                            sasiad ma miec recrystalized na false
                            if (porownanie dyslokacji obecnwj z sasiadami)
            {
                if sasiad ma wieksza -> flag = false
            }
                        }
                        
                           

                        
                        if (grid.currentArray[i, j].recristalized==false && flag)
                        {
                            for (int s = 0; s < numberOfNeighbours; s++)
                            {
                                if(grid.currentArray[i,j].currentNeighbours[s] == iteracja-1)
                                {
                                    grid.currentArray[i, j].recristalized = true;
                                    grid.currentArray[i, j].dislocationDensity = 0;
                                    grid.currentArray[i, j].currentState = grid.currentArray[i, j].currentStateOfNeighbours[s];
                                    //color
                                    grid.currentArray[i, j].iterationRecystalized = iteracja;
                                    //grid.currentArray[i, j].index = grid.currentArray[i, j].currentNeighbours[s];
                                }
                            }
                        }
                    }
                }*/




            } // <- koniec iteracji

            label7.Text = "Proces zakończył się.";
            label7.Refresh();

        }

        public void drawFromCell()
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
                        g.FillRectangle(new SolidBrush(grid.currentArray[i,j].kolor), i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }

        }

        public Form7()
        {

            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            grid = new GridGrains(setSizeX, setSizeY);
            grid = Form6.grid;
        }

        private void button2_Click(object sender, EventArgs e)// zrekrystalizowane
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
                    if (grid.currentArray[i, j].recristalized == true)
                    {
                        g.FillRectangle(new SolidBrush(Color.DarkRed), i * a + 1, j * a + 1, a - 1, a - 1);
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // drew energy
        {
            Form8 frm8 = new Form8();

            
            energyColor = new Color[numberOfNeighbours + 1];

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

            //drawEnergy();
            frm8.Show();


        }

        public void drawEnergy()
        {
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    int e = 0;
                    if(grid.currentArray[i,j].drxState == -1)
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
                                if (grid.currentArray[i, j].drxState != grid.currentArray[i,j].neighbour[s].drxState)
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

            //to przeniesc do 8?
            g.Clear(Color.White);
            pictureBox1.Refresh();


            for (int i = 0; i <= setSizeX; i++)
            {
                graf.DrawLine(pen_line, i * a, 0, i * a, setSizeY * a);

            }

            for (int i = 0; i <= setSizeY; i++)
            {
                graf.DrawLine(pen_line, 0, i * a, setSizeX * a, i * a);

            }
            pictureBox1.Image = bm;

            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    graf.FillRectangle(new SolidBrush(energyColor[grid.currentArray[i, j].energiaKomorki]), i * a + 1, j * a + 1, a - 1, a - 1);
                }
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //4215840142323,42
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.SetStyle(
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.UserPaint |
                 ControlStyles.DoubleBuffer,
                 true);

            //g.Clear(Color.White);


            
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
            grainColorTable = Form6.grainColorTable;
            grid = new GridGrains(setSizeX, setSizeY);
            grid = Form6.grid;
            
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
            draw();

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
        //TODO: energia current state + drx

        public void ComputeEnergy()
        {
            int e;
            for (int i = 0; i < setSizeX; i++)
            {
                for (int j = 0; j < setSizeY; j++)
                {
                    /*
                    int e = 0;
                    for (int s = 0; s < numberOfNeighbours; s++)
                    {
                        if (grid.currentArray[i, j].currentState != grid.currentArray[i, j].currentStateOfNeighbours[s] && grid.currentArray[i, j].currentStateOfNeighbours[s] != 0)
                        {
                            e++;
                        }
                    }
                    grid.currentArray[i, j].energiaKomorki = e;
                    */

                    e = 0;
                    if (grid.currentArray[i, j].drxState == -1 )
                    {
                        for (int s = 0; s < numberOfNeighbours; s++)
                        {
                            if (grid.currentArray[i, j].neighbour[s].drxState == -1 && grid.currentArray[i, j].neighbour[s].currentState != 0)
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
                        //grid.currentArray[i, j].energiaKomorki = e;
                    }
                    else if(grid.currentArray[i,j].drxState > 0)
                    {
                        for (int s = 0; s < numberOfNeighbours; s++)
                        {
                            if (grid.currentArray[i, j].neighbour[s].drxState == -1 && grid.currentArray[i, j].neighbour[s].currentState != 0)
                            {
                                e++;
                            }
                            else if (grid.currentArray[i, j].neighbour[s].drxState > -1)
                            {
                                if (grid.currentArray[i, j].drxState != grid.currentArray[i, j].neighbour[s].drxState)
                                {
                                    e++;
                                }
                            }
                        }
                        //grid.currentArray[i, j].energiaKomorki = e;
                    }
                    grid.currentArray[i, j].energiaKomorki = e;



                }
            }
        }



    }
}
