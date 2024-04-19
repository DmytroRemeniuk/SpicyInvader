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
using System.Windows.Media.Media3D;

namespace SpicyInvader
{
    internal class Enemy : GameObject
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Enemy(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
            _display = "(\\!/)";
        }

        #region Methods
        public new void Erase() 
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write("     ");
        }
        #endregion
    }
}
