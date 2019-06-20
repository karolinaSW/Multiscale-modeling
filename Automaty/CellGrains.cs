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

        public CellGrains(int n)
        {
            numberOfNeighbours = n;
            currentStateOfNeighbours = new int[numberOfNeighbours];
            currentState = 0;
            index = 0;

            for (int i = 0; i < numberOfNeighbours; i++)
            {
                currentStateOfNeighbours[i] = 0;

            }
        }


    }
}
