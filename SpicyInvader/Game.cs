using System;
using System.Threading;
using System.Windows.Input;
using System.Media;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SpicyInvader
{
    class Game
    {
        /// <summary>
        /// Game method
        /// </summary>
        public void NewGame()
        {
            #region Objects
            //GameObject helper = new GameObject();
            Enemy[,] enemyTable = new Enemy[Constants.ENEMY_QUANTITY_X, Constants.ENEMY_QUANTITY_Y];
            Missile playerMissile = new Missile(positionX: 0, positionY: 1, display: "|");
            Missile enemyMissile = new Missile(positionX: 0, positionY: Constants.ENEMY_MISSILE_Y, display: "\"");
            Barricade[,] barricades;
            //sounds
            
            SoundPlayer enemyDead = new SoundPlayer("..\\..\\Sounds\\enemyDead.wav");
            SoundPlayer barricadeExplosion = new SoundPlayer("..\\..\\Sounds\\barricadeExplosion.wav");
            #endregion

            #region Variables
            bool isRight = true;
            bool move = true;
            int oldX;//stock of x
            int cntrKill = 0;
            int score = 0;
            int[] xyDescend = new int[] { 0, Constants.ENEMY_QUANTITY_Y - 1 };//
            int[] xyRight = new int[] { Constants.ENEMY_QUANTITY_X - 1, 0 }; // Arrays to stock the enemies' indexes at the right, left and bottom edges
            int[] xyLeft = new int[] { 0, 0 };                              //
            //time of await of movement
            int sleep = 20;
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
                if (enemyTable[xyDescend[0], xyDescend[1]].PositionY != Constants.BARR_Y)
                {
                    //finish the game
                    if(Keyboard.IsKeyDown(Key.Escape))
                    {
                        spaceship.Lives = 0;
                    }

                    //spaceship's movement
                    oldX = spaceship.MovementControl(0);
                    spaceship.Move(oldX: oldX);

                    //shooting
                    if (Keyboard.IsKeyDown(Key.Space) && playerMissile.PositionY == 1)
                    {
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

                        for (int i = Constants.ENEMY_QUANTITY_X - 1; i >= 0; i--)
                        {
                            for (int j = Constants.ENEMY_QUANTITY_Y - 1; j >= 0; j--)
                            {
                                if (enemyTable[i, j] != null)
                                {
                                    enemyTable[i, j].Erase();
                                    enemyTable[i, j].PositionX++;
                                    //descend on the edge
                                    if (enemyTable[xyRight[0], xyRight[1]].PositionX == Constants.MAX_X)
                                    {
                                        enemyTable[i, j].PositionY++;
                                        isRight = false;
                                    }
                                    enemyTable[i, j].Write();
                                    enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j], xEnemy: Constants.ENEMY_QUANTITY_X, yEnemy: Constants.ENEMY_QUANTITY_Y);
                                }
                            }
                        }
                        move = false;
                    }
                    //change the direction
                    else if (!isRight && move)
                    {
                        xyLeft = Enemy.LeftEnemy(enemies: enemyTable, x: xyLeft[0], y: xyLeft[1]);

                        for (int i = 0; i < Constants.ENEMY_QUANTITY_X; i++)
                        {
                            for (int j = 0; j < Constants.ENEMY_QUANTITY_Y; j++)
                            {
                                if (enemyTable[i, j] != null)
                                {
                                    enemyTable[i, j].Erase();
                                    enemyTable[i, j].PositionX--;
                                    //descend on the edge
                                    if (enemyTable[xyLeft[0], xyLeft[1]].PositionX == Constants.MIN_X)
                                    {
                                        enemyTable[i, j].PositionY++;
                                        isRight = true;
                                    }
                                    enemyTable[i, j].Write();
                                    enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j], xEnemy: Constants.ENEMY_QUANTITY_X, yEnemy: Constants.ENEMY_QUANTITY_Y);
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
                    if (enemyMissile.PositionY == Constants.ENEMY_MISSILE_Y)
                    {
                        enemyMissile.Erase();
                    }

                    //two missiles' collision
                    if (playerMissile.TwoMissilesCollision(enemyMissile: enemyMissile))
                    {
                        playerMissile.Erase();
                        enemyMissile.Erase();
                        playerMissile.PositionY = 1;
                        enemyMissile.PositionY = Constants.ENEMY_MISSILE_Y;
                    }

                    //bunker's collision
                    for (int i = 0; i < Constants.BARR_QUANTITY_Y; i++)
                    {
                        for (int j = 0; j < Constants.BARR_QUANTITY_X; j++)
                        {
                            if (barricades[i, j] != null)
                            {
                                if (barricades[i, j].PositionX == enemyMissile.PositionX && barricades[i, j].PositionY == enemyMissile.PositionY ||
                                    barricades[i, j].PositionX == playerMissile.PositionX && barricades[i, j].PositionY == playerMissile.PositionY)
                                {
                                    if (barricades[i, j].PositionX == enemyMissile.PositionX)
                                    {
                                        enemyMissile.PositionY = Constants.ENEMY_MISSILE_Y;
                                    }
                                    if (barricades[i, j].PositionX == playerMissile.PositionX)
                                    {
                                        playerMissile.PositionY = 1;
                                    }

                                    barricades[i, j].Lives--;
                                    if (barricades[i, j].Lives == 0)
                                    {
                                        barricadeExplosion.Play();
                                        barricades[i, j].Erase();
                                        barricades[i, j] = null;
                                    }
                                    else
                                    {
                                        barricades[i, j].Display = "░";
                                        barricades[i, j].Write();
                                    }
                                }
                            }
                        }
                    }

                    //spaceship and enemies' missiles' collision
                    spaceship.Collision(enemyMissile);

                    //enemies and missile's collision
                    for (int i = 0; i < Constants.ENEMY_QUANTITY_X; i++)
                    {
                        for (int j = 0; j < Constants.ENEMY_QUANTITY_Y; j++)
                        {
                            if (enemyTable[i, j] != null && playerMissile.PositionY == enemyTable[i, j].PositionY && playerMissile.PositionX >= enemyTable[i, j].PositionX && playerMissile.PositionX <= enemyTable[i, j].PositionX + 4)
                            {
                                enemyDead.Play();
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
                    if (cntrKill == Constants.ENEMY_QUANTITY_X * Constants.ENEMY_QUANTITY_Y)
                    {
                        enemyTable = Enemy.CreateEnemies(enemies: enemyTable);
                        cntrKill = 0;
                        //speed up
                        if (sleep > 3)
                        {
                            sleep -= 2;
                        }
                        xyRight[0] = Constants.ENEMY_QUANTITY_X - 1;
                        xyLeft[0] = 0;
                        xyDescend[1] = Constants.ENEMY_QUANTITY_Y - 1;
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
            string name = "";

            SoundPlayer playerDead = new SoundPlayer("..\\..\\Sounds\\playerDead.wav");
            playerDead.Play();

            Console.Clear();
            //block entering of the text while playing
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            Console.SetCursorPosition(55, 20);
            Console.WriteLine("YOUR SCORE: " + score);
            Console.SetCursorPosition(53, 21);
            Console.WriteLine("ENTER YOUR NAME:");
            Console.SetCursorPosition(55, 22);
            name = Console.ReadLine();

            RegisterHighscore(currentScore: score, nickname: name);

            Console.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentScore"></param>
        private static void RegisterHighscore(int currentScore, string nickname)
        {
            string firstScore = "";
            string stockScore = "";

            for (int i = 1; i <= Directory.GetFiles(Constants.HIGHSCORE).Length; i++)
            {
                if (currentScore > Convert.ToInt32(File.ReadAllLines(Constants.HIGHSCORE + i + ".txt").ToArray()[1]))
                {
                    firstScore = File.ReadAllText(Constants.HIGHSCORE + i + ".txt");
                    File.WriteAllText(Constants.HIGHSCORE + i + ".txt", nickname + "\n" + currentScore);
                    for (int j = i; j < Directory.GetFiles(Constants.HIGHSCORE).Length; j++)
                    {
                        if(j == i)
                        {
                            stockScore = File.ReadAllText(Constants.HIGHSCORE + (j + 1) + ".txt");
                            File.WriteAllText(Constants.HIGHSCORE + (j + 1) + ".txt", firstScore);
                        }
                        else
                        {
                            firstScore = File.ReadAllText(Constants.HIGHSCORE + (j + 1) + ".txt");
                            File.WriteAllText(Constants.HIGHSCORE + (j + 1) + ".txt", stockScore);
                            stockScore = firstScore;
                        }
                    }
                    i = Directory.GetFiles(Constants.HIGHSCORE).Length;
                }
            }
        }
    }
}
