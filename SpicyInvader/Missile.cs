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
    internal class Missile : GameObject
    {
        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Missile(int positionX, int positionY, string display)
        {
            _positionX = positionX;
            _positionY = positionY;
            _display = display;
        }

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
        /// Change the Y coordinate of the player's missile
        /// </summary>
        public void MoveUp()
        {
            this.PositionY--;
        }
        /// <summary>
        /// Change the Y coordinate of the enemy's 
        /// </summary>
        public void MoveDown()
        {
            if (this.PositionY != ENEMY_MISSILE_Y)
            {
                this.PositionY++;
            }
        }
        #endregion
    }
}
