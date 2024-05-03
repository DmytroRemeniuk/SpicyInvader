/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/
using System;

namespace SpicyInvader
{
    internal class Enemy : GameObject
    {
        /// <summary>
        /// Main enemy's constructor
        /// </summary>
        /// <param name="positionX">Start X</param>
        /// <param name="positionY">Strat Y</param>
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

        /// <summary>
        /// Create the enemies
        /// </summary>
        /// <param name="enemies">Array of enemies</param>
        /// <returns>Array of enemies</returns>
        public static Enemy[,] CreateEnemies(Enemy[,] enemies)
        {
            for (int i = 0; i < Constants.ENEMY_QUANTITY_X; i++)
            {
                for (int j = 0; j < Constants.ENEMY_QUANTITY_Y; j++)
                {
                    enemies[i, j] = new Enemy(positionX: Constants.ENEMY_START_X + i * Constants.ENEMY_POS_MULTIPLIER_X, positionY: Constants.ENEMY_START_Y + j * Constants.ENEMY_POS_MULTIPLIER_Y);
                    enemies[i, j].Write();
                }
            }
            return enemies;
        }
        /// <summary>
        /// Find the indexes of the enemy at the bottom edge
        /// </summary>
        /// <param name="enemies">Enemy array</param>
        /// <param name="x">Start x index</param>
        /// <param name="y">Start y index</param>
        /// <returns>An array with indexes</returns>
        public static int[] LowerEnemy(Enemy[,] enemies, int x, int y)
        {
            int descendCounter = 0;
            int[] indexes = new int[2];

            for (int i = 0; i < Constants.ENEMY_QUANTITY_X; i++)
            {
                if (enemies[i, y] != null)
                {
                    x = i;
                    i = Constants.ENEMY_QUANTITY_X;
                }
                else
                {
                    descendCounter++;
                }

                if (y != 0 && descendCounter == Constants.ENEMY_QUANTITY_X)
                {
                    y--;
                    descendCounter = 0;
                    i = -1;
                }
            }
            indexes[0] = x;
            indexes[1] = y;
            return indexes;
        }
        /// <summary>
        /// Find the indexes of the enemy at the right edge
        /// </summary>
        /// <param name="enemies">Enemy array</param>
        /// <param name="x">Start x index</param>
        /// <param name="y">Start y index</param>
        /// <returns>An array with indexes</returns>
        public static int[] RightEnemy(Enemy[,] enemies, int x, int y)
        {
            int[] indexes = new int[2];
            int rightCounter = 0;

            for (int i = Constants.ENEMY_QUANTITY_Y - 1; i >= 0; i--)
            {
                if (enemies[x, i] != null)
                {
                    y = i;
                    i = 0;
                }
                else
                {
                    rightCounter++;
                }

                if (x != 0 && rightCounter == Constants.ENEMY_QUANTITY_Y)
                {
                    x--;
                    rightCounter = 0;
                    i = Constants.ENEMY_QUANTITY_Y;
                }
            }

            indexes[0] = x;
            indexes[1] = y;
            return indexes;
        }
        /// <summary>
        /// Find the indexes of the enemy at the left edge
        /// </summary>
        /// <param name="enemies">Enemy array</param>
        /// <param name="x">Start x index</param>
        /// <param name="y">Start y index</param>
        /// <returns>An array with indexes</returns>
        public static int[] LeftEnemy(Enemy[,] enemies, int x, int y)
        {
            int leftCounter = 0;
            int[] indexes = new int[2];

            for (int i = 0; i < Constants.ENEMY_QUANTITY_Y; i++)
            {
                if (enemies[x, i] != null)
                {
                    y = i;
                    i = Constants.ENEMY_QUANTITY_Y;
                }
                else
                {
                    leftCounter++;
                }

                if (x != Constants.ENEMY_QUANTITY_X - 1 && leftCounter == Constants.ENEMY_QUANTITY_Y)
                {
                    x++;
                    leftCounter = 0;
                    i = -1;
                }
            }

            indexes[0] = x;
            indexes[1] = y;
            return indexes;
        }
        #endregion
    }
}
