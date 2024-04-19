using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SpicyInvader
{
    internal class GameObject
    {
        //common attributes
        #region Variables
        protected int _positionX = 0;
        protected int _positionY = 0;
        protected string _display = "";
        protected int _lives = 0;
        #endregion

        #region Constants
        //limitation
        protected const int MAX_X = 120;
        protected const int MIN_X = 0;
        //for enemies
        protected const int ENEMY_QUANTITY_X = 5;
        protected const int ENEMY_QUANTITY_Y = 3;
        protected const int ENEMY_MISSILE_Y = 51;
        private const int ENEMY_START_X = 35;
        private const int ENEMY_START_Y = 10;
        private const int ENEMY_POS_MULTIPLIER_X = 8;
        private const int ENEMY_POS_MULTIPLIER_Y = 3;
        #endregion

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

        public int EnemyQuantityX
        {
            get { return ENEMY_QUANTITY_X; }
        }

        public int EnemyQuantityY
        {
            get { return ENEMY_QUANTITY_Y; }
        }

        public int EnemyMissileY
        {
            get { return ENEMY_MISSILE_Y; }
        }

        public int MinX
        {
            get { return MIN_X; }
        }

        public int MaxX
        {
            get { return MAX_X; }
        }
        #endregion

        #region Methods
        //for enemies
        /// <summary>
        /// Create the enemies
        /// </summary>
        /// <param name="enemies">Array of enemies</param>
        /// <returns>Array of enemies</returns>
        public Enemy[,] CreateEnemies(Enemy[,] enemies)
        {
            for (int i = 0; i < ENEMY_QUANTITY_X; i++)
            {
                for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                {
                    enemies[i, j] = new Enemy(positionX: ENEMY_START_X + i * ENEMY_POS_MULTIPLIER_X, positionY: ENEMY_START_Y + j * ENEMY_POS_MULTIPLIER_Y);
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
        public int[] LowerEnemy(Enemy[,] enemies, int x, int y)
        {
            int descendCounter = 0;
            int[] indexes = new int[2];
            
            for (int i = 0; i < ENEMY_QUANTITY_X; i++)
            {
                if (enemies[i, y] != null)
                {
                    x = i;
                    i = ENEMY_QUANTITY_X;
                }
                else
                {
                    descendCounter++;
                }

                if (y != 0 && descendCounter == ENEMY_QUANTITY_X)
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
        public int[] RightEnemy(Enemy[,] enemies, int x, int y)
        {
            int[] indexes = new int[2];
            int rightCounter = 0;
            
            for (int i = ENEMY_QUANTITY_Y - 1; i >= 0; i--)
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

                if (x != 0 && rightCounter == ENEMY_QUANTITY_Y)
                {
                    x--;
                    rightCounter = 0;
                    i = ENEMY_QUANTITY_Y;
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
        public int[] LeftEnemy(Enemy[,] enemies, int x, int y)
        {
            int leftCounter = 0;
            int[] indexes = new int[2];

            for (int i = 0; i < ENEMY_QUANTITY_Y; i++)
            {
                if (enemies[x, i] != null)
                {
                    y = i;
                    i = ENEMY_QUANTITY_Y;
                }
                else
                {
                    leftCounter++;
                }

                if (x != ENEMY_QUANTITY_X - 1 && leftCounter == ENEMY_QUANTITY_Y)
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

        //common
        /// <summary>
        /// Write the object's display in the right place
        /// </summary>
        public void Write()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }
        /// <summary>
        /// Erase the object on the previous position
        /// </summary>
        public void Erase()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(" ");
        }
        #endregion
    }
}
