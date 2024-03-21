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
    internal class Enemy
    {
        private string _display = "(\\!/)";
        private int _positionX = 0;
        private int _positionY = 0;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Enemy(int positionX, int positionY)
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

        public int MoveX(int x)
        {
            x++;
            return x;
        }

        public int MoveY(int y)
        {
            y++;
            return y;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write the enemy's display in the right place
        /// </summary>
        public void Write()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }

        
        #endregion
    }
}
