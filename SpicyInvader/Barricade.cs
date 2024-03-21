/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpicyInvader
{
    internal class Barricade
    {
        //attributes
        private string _display = "N";
        private int _positionX = 0;
        private int _positionY = 0;
        private int lives = 2;

        //constructor
        public Barricade(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        #region Getters&Setters
        public string Display
        {
            get { return _display; }
            set { _display = value; }
        }

        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        #endregion
    }
}
