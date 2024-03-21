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
    internal class EnemyMissile
    {
        //attributes
        #region Variables
        private int _positionX = 0;
        private int _positionY = 0;
        #endregion
        #region Constants
        private const string DISPLAY = "\"";
        private const int MAX_Y = 51;
        private const int ENEMY_QUANTITY_X = 5;
        private const int ENEMY_QUANTITY_Y = 3;
        private const int ENEMY_MISSILE_Y = 51;
        #endregion

        /// <summary>
        /// Enemy missile's constructor
        /// </summary>
        /// <param name="positionX">Enemy missile's position X</param>
        /// <param name="positionY">Enemy missile's position X</param>
        public EnemyMissile(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        #region Getters&Setters
        /// <summary>
        /// 
        /// </summary>
        public string Display
        {
            get { return DISPLAY; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }

        /// <summary>
        /// Get the max position Y
        /// </summary>
        public int MaxY
        {
            get { return MAX_Y; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start of the shooting
        /// </summary>
        /// <param name="i">from for cycle</param>
        /// <param name="j">from for cycle</param>
        /// <param name="enemy">enemy from the array</param>
        public void EnemyShoot(int i, int j, Enemy enemy)
        {
            Random random = new Random();
            int enemyX = random.Next(ENEMY_QUANTITY_X);
            int enemyY = random.Next(ENEMY_QUANTITY_Y);

            //enemies' shooting
            if (this.PositionY == ENEMY_MISSILE_Y && enemyX == i && enemyY == j && enemy.Display != "     ")
            {
                this.PositionY = enemy.PositionY + 1;
                this.PositionX = enemy.PositionX + 2;
            }
        }
        /// <summary>
        /// Change the Y coordinate
        /// </summary>
        public void Move()
        {
            if (this.PositionY != ENEMY_MISSILE_Y)
            {
                this.PositionY++;
            }
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
        /// Write/display on a new position
        /// </summary>
        public void Write()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }
        #endregion
    }
}
