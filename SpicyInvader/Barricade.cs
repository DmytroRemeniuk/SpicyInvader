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
        //constructor
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

    }
}
