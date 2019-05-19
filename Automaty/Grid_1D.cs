using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaty
{

    public class Grid_1D
    {

        //Rule r;
        private static int size;
        public Cell[] newArray = new Cell[size];
        public Cell[] currentArray = new Cell[size];
        int r;

        /*public static int Size { get => Size1; set => Size1 = value; }
        public static int Size1 { get => size; set => size = value; }
        public static int Size2 { get => size; set => size = value; }*/

        public Grid_1D(int s, int r)//Rule rule)
        {
            this.r = r;

            size = s;
            this.newArray = new Cell[size];
            this.currentArray = new Cell[size];

            initialFill(r);
            
            checkNeighbours();
            computeNewStates(r);
        }

        public void initialFill(int r)//Rule rule)
        {
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                this.currentArray[i] = new Cell(Form1.setNeighbours);
                this.newArray[i] = new Cell(Form1.setNeighbours);
                this.currentArray[i].index = i;
                this.newArray[i].index = i;

                int randomNumber = random.Next(0, 2);
                if (randomNumber == 0) //(i % 2 == 1)//(randomNumber == 0)
                {
                    this.currentArray[i].currentState = false;
                }
                else
                {
                    this.currentArray[i].currentState = true;
                }
                newArray[i].currentState = false;
                //newArray[i].currentState = false;
                //newArray[i].newState = false;

            
            }

            /*currentArray[0].currentState = true;
            currentArray[1].currentState = true;
            currentArray[2].currentState = true;
            currentArray[3].currentState = false;
            currentArray[4].currentState = true;
            currentArray[5].currentState = false;
            currentArray[6].currentState = false;
            currentArray[7].currentState = true;
            currentArray[8].currentState = true;
            currentArray[9].currentState = false;
            currentArray[10].currentState = true;
            currentArray[11].currentState = false;
            currentArray[12].currentState = false;
            currentArray[13].currentState = false;
            currentArray[14].currentState = true;
            currentArray[15].currentState = false;
            currentArray[16].currentState = false;
            currentArray[17].currentState = false;*/


            checkNeighbours();
            computeNewStates(r);
        }

        public void checkNeighbours()
        {
            for (int i = 0; i < size; i++)
            {

                if (i == 0)
                {
                    currentArray[i].currentStateOfNeighbours[0] = currentArray[size - 1].currentState;
                    currentArray[i].currentStateOfNeighbours[1] = currentArray[i + 1].currentState;
                }
                else if (i == size - 1)
                {
                    currentArray[i].currentStateOfNeighbours[0] = currentArray[i - 1].currentState;
                    currentArray[i].currentStateOfNeighbours[1] = currentArray[0].currentState;
                }
                else
                {
                    currentArray[i].currentStateOfNeighbours[0] = currentArray[i - 1].currentState;
                    currentArray[i].currentStateOfNeighbours[1] = currentArray[i + 1].currentState;
                }
            }
        }

        public void computeNewStates(int r)//Rule rule)
        {
            int intBin = r;
            int num;

            int[] tab = new int[8]; // zero element is the rightest digit of intBin

            for(int i = 0; i < 8; i++)
            {
                num = intBin % 10;
                intBin /= 10;
                tab[i] = num;
            }

            for (int i = 0; i < size; i++)
            {
                if(currentArray[i].currentState == false && currentArray[i].currentStateOfNeighbours[0] == false && currentArray[i].currentStateOfNeighbours[1] == false)
                {
                    if (tab[0] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == false && currentArray[i].currentStateOfNeighbours[0] == false && currentArray[i].currentStateOfNeighbours[1] == true)
                {
                    if (tab[1] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == true && currentArray[i].currentStateOfNeighbours[0] == false && currentArray[i].currentStateOfNeighbours[1] == false)
                {
                    if (tab[2] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == true && currentArray[i].currentStateOfNeighbours[0] == false && currentArray[i].currentStateOfNeighbours[1] == true)
                {
                    if (tab[3] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == false && currentArray[i].currentStateOfNeighbours[0] == true && currentArray[i].currentStateOfNeighbours[1] == false)
                {
                    if (tab[4] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == false && currentArray[i].currentStateOfNeighbours[0] == true && currentArray[i].currentStateOfNeighbours[1] == true)
                {
                    if (tab[5] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == true && currentArray[i].currentStateOfNeighbours[0] == true && currentArray[i].currentStateOfNeighbours[1] == false)
                {
                    if (tab[6] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
                if (currentArray[i].currentState == true && currentArray[i].currentStateOfNeighbours[0] == true && currentArray[i].currentStateOfNeighbours[1] == true)
                {
                    if (tab[7] == 0) newArray[i].currentState = false;
                    else newArray[i].currentState = true;
                }
            }

            /*for (int i = 0; i < size; i++)
            {
                newArray[i].currentState = newArray[i].currentState;
            }*/

        }

        public int getSize()
        {
            return size;
        }

        public int getR()
        {
            return r;
        }


    }
}
