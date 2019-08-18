using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace Automaty
{
    public class CellGrainsBase
    {
        public int index;
        public int currentState; //number of grain
        public int drxState; //number of grain

        public int[] currentStateOfNeighbours;
        //public int[] DislocationOfNeighbours;
        //public CellGrains[] neighbour;

        public static int numberOfNeighbours;
        public float insidePointX;
        public float insidePointY;
        //public List<KeyValuePair<int, int>> currentStateOfNeighboursList = new List<KeyValuePair<int, int>>();
        public int energiaKomorki;
        public double dislocationDensity;
        public bool recristalized;
        public int iterationRecystalized;
        public Color kolor;

        public CellGrainsBase()
        {
            currentState = 0;
            index = 0;
            insidePointX = 0;
            insidePointY = 0;
            energiaKomorki = -1;
            recristalized = false;
            dislocationDensity = 0;
            kolor = new Color();
            drxState = -1;
            iterationRecystalized = -2;
        }
    }
}
