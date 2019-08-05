using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Automaty
{
    public class CellGrains: CellGrainsBase
    {
        
        //public int index;
        //public int currentState; //number of grain
        //public int drxState; //number of grain

        //public int[] currentStateOfNeighbours;
        //public int[] DislocationOfNeighbours;
        public CellGrainsBase[] neighbour;

        //public static int numberOfNeighbours;
        //public float insidePointX;
        //public float insidePointY;
        //public List<KeyValuePair<int, int>> currentStateOfNeighboursList = new List<KeyValuePair<int, int>>();
        //public int energiaKomorki;
        //public double dislocationDensity;
        //public bool recristalized;
        //public int iterationRecystalized;
        //public Color kolor;

        
        public CellGrains(int n)
        {
            // if (n == 100)
            //{
                numberOfNeighbours = n;
                currentStateOfNeighbours = new int[numberOfNeighbours];
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
                neighbour = new CellGrainsBase[numberOfNeighbours];
                for (int i = 0; i < n; i++)
                {
                    //neighbour[i] = new CellGrains(n);
                    neighbour[i] = new CellGrainsBase();

                }
            //}
            /*else
            {

                numberOfNeighbours = n;
                currentStateOfNeighbours = new int[numberOfNeighbours];

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

                for(int i =0; i < n; i++)
                {
                    neighbour[i] = new CellGrains(n);

                }





                for (int i = 0; i < numberOfNeighbours; i++)
                {
                    currentStateOfNeighbours[i] = 0;
                    //currentNeighbours[i] = new CellGrains(numberOfNeighbours);
                }
            }*/
        }

        public CellGrains()
        {
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
        }


    }
}
