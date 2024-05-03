/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/
using System;
using System.Media;

namespace SpicyInvader
{
    internal class Missile : GameObject
    {
        SoundPlayer playerShoot = new SoundPlayer("..\\..\\Sounds\\playerShoot.wav");

        /// <summary>
        /// Missile's constructor
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
            playerShoot.Play();
            this.PositionX = spaceship.PositionX + 2;
            this.PositionY = spaceship.PositionY - 1;
        }
        /// <summary>
        /// Start of the shooting
        /// </summary>
        /// <param name="i">from "for" cycle</param>
        /// <param name="j">from "for" cycle</param>
        /// <param name="xEnemy">Enemies' quantity X</param>
        /// <param name="yEnemy">Enemies' quantity Y</param>
        /// <param name="enemy">enemy from the array</param>
        public void EnemyShoot(int i, int j, Enemy enemy, int xEnemy, int yEnemy)
        {
            Random random = new Random();
            int enemyX = random.Next(xEnemy);
            int enemyY = random.Next(yEnemy);

            //enemies' shooting
            if (this.PositionY == Constants.ENEMY_MISSILE_Y && enemyX == i && enemyY == j)
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
            if (this.PositionY != Constants.ENEMY_MISSILE_Y)
            {
                this.PositionY++;
            }
        }
        /// <summary>
        /// Verify the collision between two missiles
        /// </summary>
        /// <param name="enemyMissile">Enemy's missile</param>
        /// <returns>True or false</returns>
        public bool TwoMissilesCollision(Missile enemyMissile)
        {
            return (this.PositionX == enemyMissile.PositionX) && (this.PositionY >= enemyMissile.PositionY && this.PositionY <= enemyMissile.PositionY + 1);
        }
        #endregion
    }
}
