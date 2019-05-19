using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty // 1D
{

    public class Cell
    {
        public static int numberOfNeighbours; // = Form1.setNeighbours;
        public bool currentState;
        //public bool newState;
        public int index;
        public bool[] currentStateOfNeighbours;
        //bool[] newStateOfNeighbours;

        //for game of life
        public int numberOfAliveNeighbours;
        public int numberOfDeadNeighbours;



        public Cell(int n)
        {
            numberOfNeighbours = n;
            //numberOfNeighbours = Form1.setNeighbours;
            currentStateOfNeighbours = new bool[numberOfNeighbours]; // sasiedztwo Moora, liczone od gornego [2D] albo lewego [1D] elementu w kierunku wskazowek zegara
            //newStateOfNeighbours = new bool[n]; // sasiedztwo Moora, liczone od gornego [2D] albo lewego [1D] elementu w kierunku wskazowek zegara

            currentState = false;
            //newState = false;
            index = 0;

            for (int i = 0; i < numberOfNeighbours; i++)
            {
                currentStateOfNeighbours[i] = false;
            }

            numberOfAliveNeighbours = 0; //default
            numberOfDeadNeighbours = 0;
        }
    }

  
}
