using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;
using System.Media;

namespace SpicyInvader
{
    class Game
    {
        #region Constants
        //barricades
        private const int BARR_QUANTITY_Y = 3;
        private const int BARR_QUANTITY_X = 90;
        private const int BARR_Y = 45;
        //enemies
        private const int ENEMY_MISSILE_Y = 51;
        private const int ENEMY_QUANTITY_X = 8;
        private const int ENEMY_QUANTITY_Y = 2;
        //limitation
        private const int MAX_X = 120;
        private const int MIN_X = 0;
        #endregion

        /// <summary>
        /// Game method
        /// </summary>
        public void NewGame()
        {
            #region Objects
            //GameObject helper = new GameObject();
            Enemy[,] enemyTable = new Enemy[ENEMY_QUANTITY_X, ENEMY_QUANTITY_Y];
            Missile playerMissile = new Missile(positionX: 0, positionY: 1, display: "|");
            Missile enemyMissile = new Missile(positionX: 0, positionY: ENEMY_MISSILE_Y, display: "\"");
            Barricade[,] barricades;
            //sounds
            SoundPlayer enemyShoot = new SoundPlayer();
            SoundPlayer enemyDead = new SoundPlayer();
            SoundPlayer playerShoot = new SoundPlayer("..\\..\\Sounds\\playerShoot.wav");
            SoundPlayer playerHit = new SoundPlayer();
            SoundPlayer playerDead = new SoundPlayer();
            SoundPlayer barricadeExplosion = new SoundPlayer();
            #endregion

            #region Variables
            bool isRight = true;
            bool move = true;
            int oldX;//stock of x
            int cntrKill = 0;
            int score = 0;
            int[] xyDescend = new int[] { 0, ENEMY_QUANTITY_Y - 1 };//
            int[] xyRight = new int[] { ENEMY_QUANTITY_X - 1, 0 }; // Arrays to stock the enemies' indexes at the right, left and bottom edges
            int[] xyLeft = new int[] { 0, 0 };                       //
            //time of await of movement
            int sleep = 3;
            #endregion

            Console.Clear();
            //score
            Console.Write("Score: " + score);
            //create a spaceship
            Spaceship spaceship = new Spaceship();
            //lives
            Console.SetCursorPosition(112, 0);
            Console.Write("Lives: " + spaceship.Lives);
            //display in a specific location
            Console.SetCursorPosition(spaceship.PositionX, spaceship.PositionY);
            //display a spaceship
            Console.WriteLine(spaceship.Display);

            enemyTable = Enemy.CreateEnemies(enemies: enemyTable);

            barricades = Barricade.Create();

            //moving the objects
            while (spaceship.Lives > 0)
            {
                xyDescend = Enemy.LowerEnemy(enemies: enemyTable, x: xyDescend[0], y: xyDescend[1]);
                if (enemyTable[xyDescend[0], xyDescend[1]].PositionY != BARR_Y)
                {
                    //spaceship's movement
                    oldX = spaceship.MovementControl(0);
                    spaceship.Move(oldX: oldX);

                    //shooting
                    if (Keyboard.IsKeyDown(Key.Space) && playerMissile.PositionY == 1)
                    {
                        playerShoot.Play();
                        playerMissile.Shoot(spaceship: spaceship);
                    }

                    //missile's moving
                    if (playerMissile.PositionY > 1)
                    {
                        if (playerMissile.PositionY < spaceship.PositionY - 1)
                        {
                            playerMissile.Erase();
                        }
                        playerMissile.MoveUp();
                        playerMissile.Write();
                        if (playerMissile.PositionY == 1)
                        {
                            playerMissile.Erase();
                        }
                    }

                    //enemies' movement
                    if (isRight && move)
                    {
                        xyRight = Enemy.RightEnemy(enemyTable, x: xyRight[0], y: xyRight[1]);

                        for (int i = ENEMY_QUANTITY_X - 1; i >= 0; i--)
                        {
                            for (int j = ENEMY_QUANTITY_Y - 1; j >= 0; j--)
                            {
                                if (enemyTable[i, j] != null)
                                {
                                    enemyTable[i, j].Erase();
                                    enemyTable[i, j].PositionX++;
                                    //descend on the edge
                                    if (enemyTable[xyRight[0], xyRight[1]].PositionX == MAX_X)
                                    {
                                        enemyTable[i, j].PositionY++;
                                        isRight = false;
                                    }
                                    enemyTable[i, j].Write();
                                    enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j], xEnemy: ENEMY_QUANTITY_X, yEnemy: ENEMY_QUANTITY_Y);
                                }
                            }
                        }
                        move = false;
                    }
                    //change the direction
                    else if (!isRight && move)
                    {
                        xyLeft = Enemy.LeftEnemy(enemies: enemyTable, x: xyLeft[0], y: xyLeft[1]);

                        for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                        {
                            for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                            {
                                if (enemyTable[i, j] != null)
                                {
                                    enemyTable[i, j].Erase();
                                    enemyTable[i, j].PositionX--;
                                    //descend on the edge
                                    if (enemyTable[xyLeft[0], xyLeft[1]].PositionX == MIN_X)
                                    {
                                        enemyTable[i, j].PositionY++;
                                        isRight = true;
                                    }
                                    enemyTable[i, j].Write();
                                    enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j], xEnemy: ENEMY_QUANTITY_X, yEnemy: ENEMY_QUANTITY_Y);
                                }
                            }
                        }
                        move = false;
                    }
                    //move two times more slowly than the spaceship
                    else if (!move)
                    {
                        move = true;
                    }

                    //enemies' shooting
                    enemyMissile.Erase();
                    enemyMissile.MoveDown();
                    enemyMissile.Write();
                    if (enemyMissile.PositionY == ENEMY_MISSILE_Y)
                    {
                        enemyMissile.Erase();
                    }

                    //bunker's collision
                    for (int i = 0; i < BARR_QUANTITY_Y; i++)
                    {
                        for (int j = 0; j < BARR_QUANTITY_X; j++)
                        {
                            if (barricades[i, j] != null)
                            {
                                if (barricades[i, j].PositionX == enemyMissile.PositionX && barricades[i, j].PositionY == enemyMissile.PositionY ||
                                    barricades[i, j].PositionX == playerMissile.PositionX && barricades[i, j].PositionY == playerMissile.PositionY)
                                {
                                    if (barricades[i, j].PositionX == enemyMissile.PositionX)
                                    {
                                        enemyMissile.PositionY = ENEMY_MISSILE_Y;
                                    }
                                    if (barricades[i, j].PositionX == playerMissile.PositionX)
                                    {
                                        playerMissile.PositionY = 1;
                                    }

                                    barricades[i, j].Lives--;
                                    if (barricades[i, j].Lives == 0)
                                    {
                                        barricades[i, j].Erase();
                                        barricades[i, j] = null;
                                    }
                                    else
                                    {
                                        barricades[i, j].Display = "n";
                                        barricades[i, j].Write();
                                    }
                                }
                            }
                        }
                    }

                    //spaceship and enemies' missiles' collision
                    spaceship.Collision(enemyMissile);

                    //enemies and missile's collision
                    for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                    {
                        for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                        {
                            if (enemyTable[i, j] != null && playerMissile.PositionY == enemyTable[i, j].PositionY && playerMissile.PositionX >= enemyTable[i, j].PositionX && playerMissile.PositionX <= enemyTable[i, j].PositionX + 4)
                            {
                                enemyTable[i, j].Erase();
                                enemyTable[i, j] = null;
                                playerMissile.PositionY = 1;
                                score++;
                                Console.SetCursorPosition(7, 0);
                                Console.Write(score);
                                cntrKill++;
                            }
                        }
                    }

                    //reset the enemies
                    if (cntrKill == ENEMY_QUANTITY_X * ENEMY_QUANTITY_Y)
                    {
                        enemyTable = Enemy.CreateEnemies(enemies: enemyTable);
                        cntrKill = 0;
                        //speed up
                        if (sleep > 3)
                        {
                            sleep -= 2;
                        }
                        xyRight[0] = ENEMY_QUANTITY_X - 1;
                        xyLeft[0] = 0;
                        xyDescend[1] = ENEMY_QUANTITY_Y - 1;
                    }

                    //wait for *sleep* milliseconds to continue the cycle
                    Thread.Sleep(sleep);
                }
                else
                {
                    spaceship.Lives = 0;
                }
            }
            GameOver(score: score);
        }

        /// <summary>
        /// Display the score after the game
        /// </summary>
        /// <param name="score">Player's score</param>
        private static void GameOver(int score)
        {
            Console.Clear();
            Console.SetCursorPosition(55, 20);
            Console.WriteLine("YOUR SCORE: " + score);
            Console.SetCursorPosition(53, 21);
            Console.WriteLine("ENTER YOUR NAME:");
            Console.SetCursorPosition(55, 22);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
