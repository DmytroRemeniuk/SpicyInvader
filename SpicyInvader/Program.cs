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
using System.Threading;
using System.Windows.Input;
using System.Diagnostics;

namespace SpicyInvader
{
    internal class Program
    {
        #region Constants
        //for the size of window
        private const int WINDOW_WIDTH = 125;
        private const int WINDOW_HEIGHT = 55;
        //for enemies
        private const int ENEMY_START_X = 35;
        private const int ENEMY_START_Y = 10;
        private const int ENEMY_QUANTITY_X = 5;
        private const int ENEMY_QUANTITY_Y = 3;
        private const int ENEMY_MISSILE_Y = 51;
        //time of await of movement
        private const int SLEEP = 20;
        //limitation
        private const int MAX_X = 120;
        private const int MIN_X = 0;
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            string menuChoice;//to recover the user's input

            //set the size of the console
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            //hide the cursor
            Console.CursorVisible = false;

            //menu and a loop to continue the game
            do
            {
                //write the menu's content
                Console.WriteLine("    Menu\n" +
                                  "[1] Jouer\n" +
                                  "[2] Options\n" +
                                  "[3] Voir le highscore\n" +
                                  "[4] A propos\n" +
                                  "[Q] Quitter");
                menuChoice = Console.ReadLine().ToUpper();//recover the choice 
                //make sure the answer is appropriate to the menu content
                while(menuChoice != "1" && menuChoice != "2" && menuChoice != "3" && menuChoice != "4" && menuChoice != "Q")
                {
                    Console.WriteLine("Veuillez choisir une des options du menu");
                    menuChoice = Console.ReadLine().ToUpper();
                }

                //verify the choice and pass to the chosen stage
                switch(menuChoice)
                {
                    case "1":
                        Game();
                        ContinueQuit();
                        break;
                    case "2":
                        Console.WriteLine("Options");
                        ContinueQuit();
                        break ;
                    case "3":
                        Console.WriteLine("Highscore");
                        ContinueQuit();
                        break;
                    case "4":
                        Console.WriteLine("A propos");
                        ContinueQuit();
                        break;
                }
            }while (menuChoice != "Q");//quit if "Q"

            //method that asks every time if the user wants to continue or quit
            void ContinueQuit()
            {
                Console.WriteLine("Continuer [C] ou quitter [q]?");
                menuChoice = Console.ReadLine().ToUpper();
                //make sure that only "C" or "Q" will be entered
                while (menuChoice != "C" && menuChoice != "Q")
                {
                    Console.WriteLine("Veuillez choisir une des options");
                    menuChoice = Console.ReadLine().ToUpper();
                }
            } 
        }

        /// <summary>
        /// Game method
        /// </summary>
        public static void Game()
        {
            #region Variables
            bool rightLeft = true;
            bool move = true;
            int oldX;//stock of x
            int cptrKill = 0;
            int score = 0;
            #endregion

            #region Objects
            Enemy[,] enemyTable = new Enemy[ENEMY_QUANTITY_X, ENEMY_QUANTITY_Y];
            Enemy enemy = new Enemy(ENEMY_START_X, ENEMY_START_Y);
            Missile missile = new Missile(0, 0);
            EnemyMissile enemyMissile = new EnemyMissile(0, ENEMY_MISSILE_Y);
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

            //create the enemies
            for (int i = 0; i < ENEMY_QUANTITY_X; i++)
            {
                for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                {
                    enemy = new Enemy(positionX: ENEMY_START_X + i * 8, positionY: ENEMY_START_Y + j * 3);
                    enemy.Write();
                    enemyTable[i, j] = enemy;
                    Thread.Sleep(1);
                }
            }

            //moving the objects
            while (enemyTable[ENEMY_QUANTITY_X - 1, ENEMY_QUANTITY_Y - 1].PositionY != spaceship.PositionY && spaceship.Lives > 0)
            {
                //spaceship's movement
                oldX = spaceship.MovementControl(0);
                spaceship.Move(oldX : oldX);

                //shooting
                if (Keyboard.IsKeyDown(Key.Space) && missile.PositionY == 0)
                {
                    missile.Shoot(spaceship : spaceship);
                }

                //missile's moving
                if (missile.PositionY > 0)
                {
                    if (missile.PositionY < spaceship.PositionY - 1)
                    {
                        missile.Erase();
                    }
                    missile.Move();
                    missile.Write();
                    if (missile.PositionY == 0)
                    {
                        missile.Erase();
                    }
                }

                //enemies' movement
                if (rightLeft && move)
                {
                    for (int i = ENEMY_QUANTITY_X-1; i >= 0; i--)
                    {
                        for (int j = ENEMY_QUANTITY_Y-1; j >= 0; j--)
                        {
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write("     ");
                            enemyTable[i, j].PositionX++;
                            //descend on the edge
                            if (enemyTable[ENEMY_QUANTITY_X-1, ENEMY_QUANTITY_Y-1].PositionX == MAX_X)
                            {
                                enemyTable[i, j].PositionY++;
                                rightLeft = false;
                            }
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write(enemyTable[i, j].Display);
                            enemyMissile.EnemyShoot(i : i, j : j, enemy : enemyTable[i,j]);
                        }
                    }
                    move = false;
                }
                //change the direction
                else if(!rightLeft && move)
                {
                    for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                    {
                        for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                        {
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write("     ");
                            enemyTable[i, j].PositionX--;
                            //descend on the edge
                            if (enemyTable[0, 0].PositionX == MIN_X)
                            {
                                enemyTable[i, j].PositionY++;
                                rightLeft = true;
                            }
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write(enemyTable[i, j].Display);
                            enemyMissile.EnemyShoot(i : i, j : j, enemy : enemyTable[i, j]);
                        }
                    }
                    move = false;
                }
                //move two times more slowly than the spaceship
                else if(!move)
                {
                    move = true;
                }

                //enemies' shooting
                enemyMissile.Erase();
                enemyMissile.Move();
                enemyMissile.Write();
                if(enemyMissile.PositionY == ENEMY_MISSILE_Y)
                {
                    enemyMissile.Erase();
                }
                //spaceship and enemies' missiles' collision
                spaceship.Collision(enemyMissile);

                //enemies and blast's collision
                for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                {
                    for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                    {
                        if (enemyTable[i, j].Display != "     " && missile.PositionY == enemyTable[i, j].PositionY && missile.PositionX >= enemyTable[i, j].PositionX && missile.PositionX <= enemyTable[i, j].PositionX + 4)
                        {
                            enemyTable[i, j].Display = "     ";
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write(enemyTable[i, j].Display);
                            missile.PositionY = 0;
                            score++;
                            cptrKill++;
                        }
                    }
                }
                //reset the enemies
                if(cptrKill == ENEMY_QUANTITY_X * ENEMY_QUANTITY_Y)
                {
                    for(int i = 0; i < ENEMY_QUANTITY_X; i++)
                    {
                        for(int j = 0; j < ENEMY_QUANTITY_Y; j++)
                        {
                            enemyTable[i, j].Display = "(\\!/)";
                            enemyTable[i, j].PositionY = ENEMY_START_Y + j * 3;
                        }
                    }
                    cptrKill = 0;
                }
                Console.SetCursorPosition(0, 0);
                Console.Write("Score: " + score);
                Console.SetCursorPosition(112, 0);
                Console.Write("Lives: " + spaceship.Lives);

                //wait for *SLEEP* milliseconds to continur the cycle
                Thread.Sleep(SLEEP);
            }
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
