using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpicyInvader
{
    internal class Spaceship
    {
        //attributes
        private const string DISPLAY = "«=ˆ=»";
        private int _positionX = 100;
        private const int _positionY = 50;
        private int lives = 300000;
        //limitation
        private const int MAX_X = 120;
        private const int MIN_X = 0;
        

        #region Getters&Setters
        public int PositionX
        { 
            get { return _positionX; } 
            set { _positionX = value; } 
        }
        public int PositionY
        {
            get { return _positionY; }
        }
        public string Display
        { 
            get { return DISPLAY; }
        }
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Change the coordinates of the spaceship
        /// </summary>
        /// <param name="oldX"></param>
        /// <returns></returns>
        public int MovementControl(int oldX)
        {
            if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
            {
                if (this.PositionX > MIN_X)
                {
                    oldX = this.PositionX;
                    this.PositionX--;
                }
            }

            if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
            {
                if (this.PositionX < MAX_X)
                {
                    oldX = this.PositionX;
                    this.PositionX++;
                }
            }
            return oldX;
        }
        /// <summary>
        /// Display the spaceship at the new coordinates
        /// </summary>
        /// <param name="oldX"></param>
        public void Move(int oldX)
        {
            Console.SetCursorPosition(oldX, this.PositionY);
            Console.Write(" ");
            if (oldX > MIN_X)
            {
                Console.SetCursorPosition(oldX - 1, this.PositionY);
                Console.Write(" ");
            }
            Console.SetCursorPosition(oldX + this.Display.Length - 1, this.PositionY);
            Console.Write(" ");
            if (oldX > MIN_X && oldX < MAX_X)
            {
                Console.SetCursorPosition(oldX + this.Display.Length, this.PositionY);
                Console.Write(" ");
            }
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }
        /// <summary>
        /// Collision between the spaceship and enemy's missile
        /// </summary>
        /// <param name="enemyMissile"></param>
        public void Collision(EnemyMissile enemyMissile)
        {
            if (enemyMissile.PositionY == this.PositionY && enemyMissile.PositionX >= this.PositionX && enemyMissile.PositionX <= this.PositionX + 4)
            {
                this.Lives--;
            }
        }
        #endregion
    }
}
