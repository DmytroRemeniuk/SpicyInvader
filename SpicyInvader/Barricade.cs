/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SpicyInvader
{
    internal class Barricade : GameObject
    {
        private const int BARR_QUANTITY_Y = 3;
        private const int BARR_QUANTITY_X = 90;
        private const int BARR_X = 20;
        private const int BARR_Y = 45;

        public Barricade(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
            _display = "N";
            _lives = 2;
        }

        #region Getters&Setters
        public int Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }
        #endregion

        public static Barricade[,] Create()
        {
            Barricade[,] barricades = new Barricade[BARR_QUANTITY_Y, BARR_QUANTITY_X];

            //create the barricades
            for (int i = 0; i < BARR_QUANTITY_Y; i++)
            {
                for (int j = 0; j < BARR_QUANTITY_X; j++)
                {
                    if (j < 8 || j > 40 && j < 49 || j > 81)
                    {
                        barricades[i, j] = new Barricade(BARR_X + j, BARR_Y + i);
                        barricades[i, j].Write();
                    }
                }
            }

            return barricades;
        }
    }
}
