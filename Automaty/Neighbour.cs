using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{
    public class Neighbour: CellGrains
    {
        //public int drxState; //number of grain
        //public double dislocationDensity;
        //public bool recristalized;
        //public int iterationRecystalized;
        //public Color kolor;

        public Neighbour(int n): base(n)
        {
            //numberOfNeighbours = n;
            //currentStateOfNeighbours = new int[numberOfNeighbours];

            currentState = 0;
            index = 0;

            insidePointX = 0;
            insidePointY = 0;
            energiaKomorki = 0;

            recristalized = false;
            dislocationDensity = 0;
            kolor = new Color();
            drxState = -1;
            iterationRecystalized = -2;

            /*for (int i = 0; i < numberOfNeighbours; i++)
            {
                neighbour[i] = new Neighbour(n);

            }*/





            for (int i = 0; i < numberOfNeighbours; i++)
            {
                currentStateOfNeighbours[i] = 0;
                //currentNeighbours[i] = new CellGrains(numberOfNeighbours);
            }


            drxState = -2;
            dislocationDensity = -2;
            recristalized = false;
            kolor = new Color();
        }

    }
}
