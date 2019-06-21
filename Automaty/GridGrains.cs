using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{
    class GridGrains
    {
        private static int sizeX, sizeY;
        public CellGrains[,] newArray; //= new Cell[sizeX, sizeY];
        public CellGrains[,] currentArray; // = new Cell[sizeX, sizeY];



        public GridGrains(int sx, int sy)
        {
            sizeX = sx;
            sizeY = sy;

            newArray = new CellGrains[sizeX, sizeY];
            currentArray = new CellGrains[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (Form5.flagDynamicNeighbour != true)
                    {
                        this.currentArray[i, j] = new CellGrains(Form5.numberOfNeighbours);
                        this.newArray[i, j] = new CellGrains(Form5.numberOfNeighbours);
                    }
                    else
                    {
                        /*
                        float cellRadius = 0;
                        int exceedNeighbours = 0;
                        // TODO: liczenie max liczby sasiadow dla zadanego promienia

                        this.currentArray[i, j] = new CellGrains(exceedNeighbours);
                        this.newArray[i, j] = new CellGrains(exceedNeighbours);
                        */

                        this.currentArray[i, j] = new CellGrains(100);
                        this.newArray[i, j] = new CellGrains(100);

                    }

                }
            }
        }

        public int getSizeX()
        {
            return sizeX;
        }
        public int getSizeY()
        {
            return sizeY;
        }

    }
}
