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
using System.Runtime.InteropServices; //for 

namespace SpicyInvader
{
    internal class Program
    {
        #region Block the resizing of console (const, import dll and methods)
        private const int MF_BYCOMMAND = 0x00000000; //false
        public const int SC_MAXIMIZE = 0xF030;       //maximize the console
        public const int SC_SIZE = 0xF000;           //resize the console

        [DllImport("user32.dll")]                                                     //
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags); //
                                                                                      //
        [DllImport("user32.dll")]                                                     //
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);        //
                                                                                      //
        [DllImport("kernel32.dll", ExactSpelling = true)]                             //
        private static extern IntPtr GetConsoleWindow();                              //
        #endregion //source - https://stackoverflow.com/questions/38426338/c-sharp-console-disable-resize

        #region Constants
        //for the size of window
        private const int WINDOW_WIDTH = 125;
        private const int WINDOW_HEIGHT = 55;
        //for enemies
        private const int ENEMY_START_X = 35;
        private const int ENEMY_START_Y = 10;
        private const int ENEMY_QUANTITY_X = 5;
        private const int ENEMY_QUANTITY_Y = 3;
        private const int ENEMY_POS_MULTIPLIER_X = 8; //8
        private const int ENEMY_POS_MULTIPLIER_Y = 3; //3
        private const int ENEMY_MISSILE_Y = 51;
        //limitation
        private const int MAX_X = 120;
        private const int MIN_X = 0;
        //barricades
        private const int BARR_QUANTITY_Y = 3;
        private const int BARR_QUANTITY_X = 6;
        private const int BARR1_X = 20;
        private const int BARR2_X = 60;
        private const int BARR3_X = 100;
        private const int BARR_Y = 45;
        #endregion

        [STAThread]
        static void Main(string[] args)
        {
            #region Block the resizing of console
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            #endregion //source - https://stackoverflow.com/questions/38426338/c-sharp-console-disable-resize

            //variables
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
            bool isRight = true;
            bool move = true;
            int oldX;//stock of x
            int cptrKill = 0;
            int score = 0;
            int xDescend = 0;
            int yDescend = ENEMY_QUANTITY_Y - 1;
            int descendCounter = 0;
            int xRight = ENEMY_QUANTITY_X - 1;
            int yRight = 0;
            int rightCounter = 0;
            int xLeft = 0;
            int yLeft = 0;
            int leftCounter = 0;
            //time of await of movement
            int sleep = 20;
            #endregion

            #region Objects
            Enemy[,] enemyTable = new Enemy[ENEMY_QUANTITY_X, ENEMY_QUANTITY_Y];
            Missile missile = new Missile(0, 0);
            EnemyMissile enemyMissile = new EnemyMissile(0, ENEMY_MISSILE_Y);
            Barricade[,] barricades1 = new Barricade[BARR_QUANTITY_Y, BARR_QUANTITY_X];
            Barricade[,] barricades2 = new Barricade[BARR_QUANTITY_Y, BARR_QUANTITY_X];
            Barricade[,] barricades3 = new Barricade[BARR_QUANTITY_Y, BARR_QUANTITY_X];
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
                    enemyTable[i, j] = new Enemy(positionX: ENEMY_START_X + i * ENEMY_POS_MULTIPLIER_X, positionY: ENEMY_START_Y + j * ENEMY_POS_MULTIPLIER_Y);
                    enemyTable[i, j].Write();
                }
            }

            //create the barricades
            for(int i = 0; i < BARR_QUANTITY_Y; i++)
            {
                for(int j = 0; j < BARR_QUANTITY_X; j++)
                {
                    barricades1[i, j] = new Barricade(BARR1_X + j, BARR_Y + i);
                    Console.SetCursorPosition(barricades1[i, j].PositionX, barricades1[i, j].PositionY);
                    Console.Write(barricades1[i, j].Display);
                }
            }
            for (int i = 0; i < BARR_QUANTITY_Y; i++)
            {
                for (int j = 0; j < BARR_QUANTITY_X; j++)
                {
                    barricades2[i, j] = new Barricade(BARR2_X + j, BARR_Y + i);
                    Console.SetCursorPosition(barricades2[i, j].PositionX, barricades2[i, j].PositionY);
                    Console.Write(barricades2[i, j].Display);
                }
            }
            for (int i = 0; i < BARR_QUANTITY_Y; i++)
            {
                for (int j = 0; j < BARR_QUANTITY_X; j++)
                {
                    barricades3[i, j] = new Barricade(BARR3_X + j, BARR_Y + i);
                    Console.SetCursorPosition(barricades3[i, j].PositionX, barricades3[i, j].PositionY);
                    Console.Write(barricades3[i, j].Display);
                }
            }

            //moving the objects
            while (spaceship.Lives > 0)
            {
                descendCounter = 0;
                for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                {
                    if (enemyTable[i, yDescend] != null)
                    {
                        xDescend = i;
                        i = ENEMY_QUANTITY_X;
                    }
                    else
                    {
                        descendCounter++;
                    }

                    if (yDescend != 0 && descendCounter == ENEMY_QUANTITY_X)
                    {
                        yDescend--;
                        descendCounter = 0;
                        i = -1;
                    }
                }
                if (enemyTable[xDescend, yDescend] != null && enemyTable[xDescend, yDescend].PositionY != spaceship.PositionY)
                {
                    Moving();
                }
                else if (enemyTable[xDescend, yDescend] != null && enemyTable[xDescend, yDescend].PositionY == spaceship.PositionY)
                {
                    spaceship.Lives = 0;
                }
            }

            Console.Clear();
            Console.SetCursorPosition(55, 20);
            Console.WriteLine("YOUR SCORE: " + score);
            Console.SetCursorPosition(53, 21);
            Console.WriteLine("ENTER YOUR NAME:");
            Console.SetCursorPosition(55, 22);
            Console.ReadLine();
            Console.Clear();

            void Moving()
            {
                //spaceship's movement
                oldX = spaceship.MovementControl(0);
                spaceship.Move(oldX: oldX);

                //shooting
                if (Keyboard.IsKeyDown(Key.Space) && missile.PositionY == 0)
                {
                    missile.Shoot(spaceship: spaceship);
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
                if (isRight && move)
                {
                    rightCounter = 0;
                    for (int i = ENEMY_QUANTITY_Y - 1; i >= 0; i--)
                    {
                        if (enemyTable[xRight, i] != null)
                        {
                            yRight = i;
                            i = 0;
                        }
                        else
                        {
                            rightCounter++;
                        }

                        if (xRight != 0 && rightCounter == ENEMY_QUANTITY_Y)
                        {
                            xRight--;
                            rightCounter = 0;
                            i = ENEMY_QUANTITY_Y;
                        }
                    }
                    for (int i = ENEMY_QUANTITY_X - 1; i >= 0; i--)
                    {
                        for (int j = ENEMY_QUANTITY_Y - 1; j >= 0; j--)
                        {
                            if (enemyTable[i, j] != null)
                            {
                                Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                                Console.Write("     ");
                                enemyTable[i, j].PositionX++;
                                //descend on the edge
                                if (enemyTable[xRight, yRight].PositionX == MAX_X)
                                {
                                    enemyTable[i, j].PositionY++;
                                    isRight = false;
                                }
                                Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                                Console.Write(enemyTable[i, j].Display);
                                enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j]);
                            }
                        }
                    }
                    move = false;
                }
                //change the direction
                else if (!isRight && move)
                {
                    leftCounter = 0;
                    for (int i = 0; i < ENEMY_QUANTITY_Y; i++)
                    {
                        if (enemyTable[xLeft, i] != null)
                        {
                            yLeft = i;
                            i = ENEMY_QUANTITY_Y;
                        }
                        else
                        {
                            leftCounter++;
                        }

                        if (xLeft != ENEMY_QUANTITY_X - 1 && leftCounter == ENEMY_QUANTITY_Y)
                        {
                            xLeft++;
                            leftCounter = 0;
                            i = -1;
                        }
                    }
                    for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                    {
                        for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                        {
                            if (enemyTable[i, j] != null)
                            {
                                Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                                Console.Write("     ");
                                enemyTable[i, j].PositionX--;
                                //descend on the edge
                                if (enemyTable[xLeft, yLeft].PositionX == MIN_X)
                                {
                                    enemyTable[i, j].PositionY++;
                                    isRight = true;
                                }
                                Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                                Console.Write(enemyTable[i, j].Display);
                                enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j]);
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
                enemyMissile.Move();
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
                        if (barricades1[i, j] != null && barricades1[i, j].PositionX == enemyMissile.PositionX && barricades1[i, j].PositionY == enemyMissile.PositionY || barricades1[i, j].Lives != 0 && barricades1[i, j].PositionX == missile.PositionX && barricades1[i, j].PositionY == missile.PositionY)
                        {
                            barricades1[i, j].Lives--;
                            if(barricades1[i, j].Lives == 0)
                            {
                                barricades1[i, j] = null;
                            }
                            else
                            {
                                barricades1[i, j].Display = "n";
                            }

                            if(barricades1[i, j] != null)
                            {
                                for (int k = 0; k < BARR_QUANTITY_Y; k++)
                                {
                                    for (int l = 0; l < BARR_QUANTITY_X; l++)
                                    {
                                        Console.SetCursorPosition(barricades1[i, j].PositionX, barricades1[i, j].PositionY);
                                        Console.Write(barricades1[i, j].Display);
                                    }
                                }
                            }

                            missile.PositionY = 0;
                        }
                    }
                }
                for (int i = 0; i < BARR_QUANTITY_Y; i++)
                {
                    for (int j = 0; j < BARR_QUANTITY_X; j++)
                    {
                        if (barricades2[i, j] != null && barricades2[i, j].PositionX == enemyMissile.PositionX && barricades2[i, j].PositionY == enemyMissile.PositionY || barricades2[i, j].Lives != 0 && barricades2[i, j].PositionX == missile.PositionX && barricades2[i, j].PositionY == missile.PositionY)
                        {
                            barricades2[i, j].Lives--;
                            if (barricades1[i, j].Lives == 0)
                            {
                                barricades2[i, j] = null;
                            }
                            else
                            {
                                barricades2[i, j].Display = "n";
                            }

                            if(barricades2[i, j] != null)
                            {
                                for (int k = 0; k < BARR_QUANTITY_Y; k++)
                                {
                                    for (int l = 0; l < BARR_QUANTITY_X; l++)
                                    {
                                        Console.SetCursorPosition(barricades2[i, j].PositionX, barricades2[i, j].PositionY);
                                        Console.Write(barricades2[i, j].Display);
                                    }
                                }
                            }
                            
                            missile.PositionY = 0;
                        }
                    }
                }
                for (int i = 0; i < BARR_QUANTITY_Y; i++)
                {
                    for (int j = 0; j < BARR_QUANTITY_X; j++)
                    {
                        if (barricades3[i, j] != null && barricades3[i, j].PositionX == enemyMissile.PositionX && barricades3[i, j].PositionY == enemyMissile.PositionY || barricades3[i, j].Lives != 0 && barricades3[i, j].PositionX == missile.PositionX && barricades3[i, j].PositionY == missile.PositionY)
                        {
                            barricades3[i, j].Lives--;
                            if (barricades3[i, j].Lives == 0)
                            {
                                barricades3[i, j] = null;
                            }
                            else
                            {
                                barricades3[i, j].Display = "n";
                            }

                            if (barricades3[i, j] != null)
                            {
                                for (int k = 0; k < BARR_QUANTITY_Y; k++)
                                {
                                    for (int l = 0; l < BARR_QUANTITY_X; l++)
                                    {
                                        Console.SetCursorPosition(barricades3[i, j].PositionX, barricades3[i, j].PositionY);
                                        Console.Write(barricades3[i, j].Display);
                                    }
                                }
                            }

                            missile.PositionY = 0;
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
                        if (enemyTable[i, j] != null && missile.PositionY == enemyTable[i, j].PositionY && missile.PositionX >= enemyTable[i, j].PositionX && missile.PositionX <= enemyTable[i, j].PositionX + 4)
                        {
                            Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                            Console.Write("     ");
                            enemyTable[i, j] = null;
                            missile.PositionY = 0;
                            score++;
                            cptrKill++;
                        }
                    }
                }

                //reset the enemies
                if (cptrKill == ENEMY_QUANTITY_X * ENEMY_QUANTITY_Y)
                {
                    for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                    {
                        for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                        {
                            enemyTable[i, j] = new Enemy(positionX: ENEMY_START_X + i * 8, positionY: ENEMY_START_Y + j * 3);
                            enemyTable[i, j].Write();
                        }
                    }
                    cptrKill = 0;
                    if(sleep > 3)
                    {
                        sleep -= 2;
                    }
                    xRight = ENEMY_QUANTITY_X - 1;
                    xLeft = 0;
                    yDescend = ENEMY_QUANTITY_Y - 1;
                }

                Console.SetCursorPosition(0, 0);
                Console.Write("Score: " + score);
                Console.SetCursorPosition(112, 0);
                Console.Write("Lives: " + spaceship.Lives);

                //wait for *sleep* milliseconds to continue the cycle
                Thread.Sleep(sleep);  
            }
        }
    }
}
