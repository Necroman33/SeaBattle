using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SeaBattle
{
    public class Bot
    {
        public int[,] myMap = new int[10, 10];
        public int[,] enemyMap = new int[10, 10];

        public Button[,] myButtons = new Button[10, 10];
        public Button[,] enemyButtons = new Button[10, 10];

        public Bot(int[,] myMap, int[,] enemyMap, Button[,] myButtons, Button[,] enemyButtons)
        {
            this.myMap = myMap;
            this.enemyMap = enemyMap;
            this.enemyButtons = enemyButtons;
            this.myButtons = myButtons;
        }



        public bool IsInsideMap(int i, int j)
        {
            if (i < 0 || j < 0 || i >= 10 || j >= 10)
            {
                return false;
            }
            return true;
        }

        public bool IsEmpty(int i, int j, int length)
        {
            bool isEmpty = true;

            for (int k = j; k < j + length; k++)
            {
                if (myMap[i, k] != 0)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        public int[,] ConfigureShips()
        {
            int lengthShip = 4;
            int cycleValue = 4;
            int shipsCount = 10;
            Random r = new Random();

            int posX = 0;
            int posY = 0;

            while (shipsCount > 0)
            {
                for (int i = 0; i < cycleValue / 4; i++)
                {
                    posX = r.Next(0, 10);
                    posY = r.Next(0, 10);

                    while (!IsInsideMap(posX, posY + lengthShip - 1) || !IsEmpty(posX, posY, lengthShip))
                    {
                        posX = r.Next(0, 10);
                        posY = r.Next(0, 10);
                    }
                    for (int k = posY; k < posY + lengthShip; k++)
                    {
                        myMap[posX, k] = 1;
                    }



                    shipsCount--;
                    if (shipsCount <= 0)
                        break;
                }
                cycleValue += 4;
                lengthShip--;
            }
            return myMap;
        }


        public bool Shoot()
        {
            bool hit = false;
            Random r = new Random();

            int posX = r.Next(0, 10);
            int posY = r.Next(0, 10);

            while (enemyButtons[posX, posY].Background == Brushes.Red || enemyButtons[posX, posY].Background == Brushes.Blue)
            {
                posX = r.Next(0, 10);
                posY = r.Next(0, 10);
            }

            if (enemyMap[posX, posY] != 0)
            {
                hit = true;
                enemyMap[posX, posY] = 3;
                enemyButtons[posX, posY].Background = Brushes.Red;
            }
            else
            {
                hit = false;
                enemyButtons[posX, posY].Background = Brushes.Blue;
            }
            if (hit)
                Shoot();
            return hit;
        }

    }
}
