using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{
    public class Grid_2D
    {
        private static int sizeX, sizeY;
        public Cell[,] newArray; //= new Cell[sizeX, sizeY];
        public Cell[,] currentArray; // = new Cell[sizeX, sizeY];



        public Grid_2D(int sx, int sy)
        {
            sizeX = sx;
            sizeY = sy;

            newArray = new Cell[sizeX, sizeY];
            currentArray = new Cell[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    this.currentArray[i, j] = new Cell(Form4.setNeighbours);
                    this.newArray[i, j] = new Cell(Form4.setNeighbours);

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
