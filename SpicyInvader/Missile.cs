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
    internal class Missile
    {
        //attributes
        private const string DISPLAY = "|";
        private int _positionX = 0;
        private int _positionY = 0;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Missile(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        #region Getters&Setters
        public string Display
        {
            get { return DISPLAY; }
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
        #endregion

        #region Methods
        /// <summary>
        /// The start of shooting
        /// </summary>
        public void Shoot(Spaceship spaceship)
        {
            this.PositionX = spaceship.PositionX + 2;
            this.PositionY = spaceship.PositionY - 1;
        }
        /// <summary>
        /// Change the Y coordinate of the missile
        /// </summary>
        public void Move()
        {
            this.PositionY--;
        }
        /// <summary>
        /// Erase the missile on the previous position
        /// </summary>
        public void Erase()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(" ");
        }
        /// <summary>
        /// Write/display the missile on the new position
        /// </summary>
        public void Write()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }
        #endregion
    }
}
