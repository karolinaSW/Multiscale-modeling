using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{
    class CellGrains
    {
        public int index;
        public int currentState; //number of grain
        public int[] currentStateOfNeighbours;
        public static int numberOfNeighbours;
        public float insidePointX;
        public float insidePointY;
        public List<KeyValuePair<int, int>> currentStateOfNeighboursList = new List<KeyValuePair<int, int>>();


        public CellGrains(int n)
        {
            if (n == 100)
            {
                currentState = 0;
                index = 0;
                insidePointX = 0;
                insidePointY = 0;

            }
            else
            {

                numberOfNeighbours = n;
                currentStateOfNeighbours = new int[numberOfNeighbours];
                currentState = 0;
                index = 0;

                insidePointX = 0;
                insidePointY = 0;


                for (int i = 0; i < numberOfNeighbours; i++)
                {
                    currentStateOfNeighbours[i] = 0;
                }
            }
        }


    }
}
