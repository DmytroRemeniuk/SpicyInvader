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
using System.Runtime.InteropServices;

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
        //title
        const string TITLE = "                              ▄▄                                                             ▄▄                  \n" +
                             "           ▄█▀▀▀█▄█           ██                  ▀████▀                                   ▀███                  \n" +
                             "          ▄██    ▀█                                 ██                                       ██                  \n" +
                             "          ▀███▄   ▀████████▄▀███  ▄██▀██▀██▀   ▀██▀ ██ ▀████████▄ ▀██▀   ▀██▀ ▄█▀██▄    ▄█▀▀███   ▄▄█▀██▀███▄███ \n" +
                             "            ▀█████▄ ██   ▀██  ██ ██▀  ██  ██   ▄█   ██   ██    ██   ██   ▄█  ██   ██  ▄██    ██  ▄█▀   ██ ██▀ ▀▀ \n" +
                             "          ▄     ▀██ ██    ██  ██ ██        ██ ▄█    ██   ██    ██    ██ ▄█    ▄█████  ███    ██  ██▀▀▀▀▀▀ ██     \n" +
                             "          ██     ██ ██   ▄██  ██ ██▄    ▄   ███     ██   ██    ██     ███    ██   ██  ▀██    ██  ██▄    ▄ ██     \n" +
                             "          █▀█████▀  ██████▀ ▄████▄█████▀    ▄█    ▄████▄████  ████▄    █     ▀████▀██▄ ▀████▀███▄ ▀█████▀████▄   \n" +
                             "                    ██                    ▄█                                                                     \n" +
                             "                    ████▄                ██▀                                                                       ";
        //barricades
        private const int BARR_QUANTITY_Y = 3;
        private const int BARR_QUANTITY_X = 90;
        private const int BARR_X = 20;
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
            string[] menuTable = new string[] { "",
                                                "Start the game",
                                                "    Options   ",
                                                "   Highscore  ",
                                                "     About    ",
                                                "     Quit     "};
            int menuChoice = 1;//to recover the user's input
            bool goon = false;

            //set the size of the console
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            //disable scrolling
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            //hide the cursor
            Console.CursorVisible = false;

            //menu and a loop to continue the game
            do
            {
                Console.Clear();
                Console.WriteLine(TITLE);
                Console.SetCursorPosition(55, 21);
                Activate();
                Console.WriteLine(menuTable[1]);
                Desactivate();
                Console.SetCursorPosition(55, 22);
                Console.WriteLine(menuTable[2]);
                Console.SetCursorPosition(55, 23);
                Console.WriteLine(menuTable[3]);
                Console.SetCursorPosition(55, 24);
                Console.WriteLine(menuTable[4]);
                Console.SetCursorPosition(55, 25);
                Console.WriteLine(menuTable[5]);

                menuChoice = 1;

                do
                {
                    switch(Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            if(menuChoice > 1)
                            {
                                menuChoice--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (menuChoice < 5)
                            {
                                menuChoice++;
                            }
                            break;
                        case ConsoleKey.Enter:
                            goon = true;
                            break;
                    }
                    Console.Clear();
                    Console.WriteLine(TITLE);
                    for(int i = 1; i < menuTable.Length; i++)
                    {
                        if(menuChoice == i)
                        {
                            Activate();
                        }
                        Console.SetCursorPosition(55, 20 + i);
                        Console.WriteLine(menuTable[i]);
                        Desactivate();
                    }
                } while (!goon);//quit the loop after pressing "Enter"

                goon = false;

                //verify the choice and pass to the chosen stage
                switch(menuChoice)
                {
                    case 1:
                        Game();
                        break;
                    case 2:
                        Options();
                        break ;
                    case 3:
                        Highscore();
                        break;
                    case 4:
                        About();
                        break;
                }
            }while (menuChoice != 5);//quit
        }
        /// <summary>
        /// Game method
        /// </summary>
        public static void Game()
        {
            #region Objects
            GameObject helper = new GameObject();
            Enemy[,] enemyTable = new Enemy[helper.EnemyQuantityX, helper.EnemyQuantityY];
            Missile playerMissile = new Missile(positionX: 0, positionY: 1, display: "|");
            Missile enemyMissile = new Missile(positionX: 0, positionY: helper.EnemyMissileY, display: "\"");
            Barricade[,] barricades = new Barricade[BARR_QUANTITY_Y, BARR_QUANTITY_X];
            #endregion

            #region Variables
            bool isRight = true;
            bool move = true;
            int oldX;//stock of x
            int cntrKill = 0;
            int score = 0;
            int[] xyDescend = new int[]{ 0, helper.EnemyQuantityY - 1 };//
            int[] xyRight = new int[] { helper.EnemyQuantityX - 1, 0 }; // Arrays to stock the enemies' indexes at the right, left and bottom edges
            int[] xyLeft = new int[]{ 0, 0};                       //
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

            enemyTable = helper.CreateEnemies(enemies: enemyTable);

            //create the barricades
            for(int i = 0; i < BARR_QUANTITY_Y; i++)
            {
                for(int j = 0; j < BARR_QUANTITY_X; j++)
                {
                    if(j < 8 || j > 40 && j < 49 || j > 81)
                    {
                        barricades[i, j] = new Barricade(BARR_X + j, BARR_Y + i);
                        Console.SetCursorPosition(barricades[i, j].PositionX, barricades[i, j].PositionY);
                        Console.Write(barricades[i, j].Display);
                    }
                }
            }

            //moving the objects
            while (spaceship.Lives > 0)
            {
                xyDescend = helper.LowerEnemy(enemies: enemyTable, x: xyDescend[0], y: xyDescend[1]);
                if (enemyTable[xyDescend[0], xyDescend[1]].PositionY != BARR_Y)
                {
                    MovingObjects();
                }
                else
                {
                    spaceship.Lives = 0;
                }
            }
            //game over
            Console.Clear();
            Console.SetCursorPosition(55, 20);
            Console.WriteLine("YOUR SCORE: " + score);
            Console.SetCursorPosition(53, 21);
            Console.WriteLine("ENTER YOUR NAME:");
            Console.SetCursorPosition(55, 22);
            Console.ReadLine();
            Console.Clear();

            void MovingObjects()
            {
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
                    xyRight = helper.RightEnemy(enemyTable, x: xyRight[0], y: xyRight[1]);

                    for (int i = helper.EnemyQuantityX - 1; i >= 0; i--)
                    {
                        for (int j = helper.EnemyQuantityY - 1; j >= 0; j--)
                        {
                            if (enemyTable[i, j] != null)
                            {
                                enemyTable[i, j].Erase();
                                enemyTable[i, j].PositionX++;
                                //descend on the edge
                                if (enemyTable[xyRight[0], xyRight[1]].PositionX == helper.MaxX)
                                {
                                    enemyTable[i, j].PositionY++;
                                    isRight = false;
                                }
                                enemyTable[i, j].Write();
                                enemyMissile.EnemyShoot(i: i, j: j, enemy: enemyTable[i, j]);
                            }
                        }
                    }
                    move = false;
                }
                //change the direction
                else if (!isRight && move)
                {
                    xyLeft = helper.LeftEnemy(enemies: enemyTable, x: xyLeft[0], y: xyLeft[1]);

                    for (int i = 0; i < helper.EnemyQuantityX; i++)
                    {
                        for (int j = 0; j < helper.EnemyQuantityY; j++)
                        {
                            if (enemyTable[i, j] != null)
                            {
                                enemyTable[i, j].Erase();
                                enemyTable[i, j].PositionX--;
                                //descend on the edge
                                if (enemyTable[xyLeft[0], xyLeft[1]].PositionX == helper.MinX)
                                {
                                    enemyTable[i, j].PositionY++;
                                    isRight = true;
                                }
                                enemyTable[i, j].Write();
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
                enemyMissile.MoveDown();
                enemyMissile.Write();
                if (enemyMissile.PositionY == helper.EnemyMissileY)
                {
                    enemyMissile.Erase();
                }

                //bunker's collision
                for (int i = 0; i < BARR_QUANTITY_Y; i++)
                {
                    for (int j = 0; j < BARR_QUANTITY_X; j++)
                    {
                        if(barricades[i, j] != null)
                        {
                            if (barricades[i, j].PositionX == enemyMissile.PositionX && barricades[i, j].PositionY == enemyMissile.PositionY ||
                                barricades[i, j].PositionX == playerMissile.PositionX && barricades[i, j].PositionY == playerMissile.PositionY)
                            {
                                if (barricades[i, j].PositionX == enemyMissile.PositionX)
                                {
                                    enemyMissile.PositionY = helper.EnemyMissileY;
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
                for (int i = 0; i < helper.EnemyQuantityX; i++)
                {
                    for (int j = 0; j < helper.EnemyQuantityY; j++)
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
                if (cntrKill == helper.EnemyQuantityX * helper.EnemyQuantityY)
                {
                    enemyTable = helper.CreateEnemies(enemies: enemyTable);
                    cntrKill = 0;
                    //speed up
                    if(sleep > 3)
                    {
                        sleep -= 2;
                    }
                    xyRight[0] = helper.EnemyQuantityX - 1;
                    xyLeft[0] = 0;
                    xyDescend[1] = helper.EnemyQuantityY - 1;
                }

                //wait for *sleep* milliseconds to continue the cycle
                Thread.Sleep(sleep);  
            }
        }
        /// <summary>
        /// To display the options
        /// </summary>
        public static void Options()
        {
            Console.Clear();
            Console.WriteLine("Options");
            Console.ReadLine();
        }
        /// <summary>
        /// To display the highscores
        /// </summary>
        public static void Highscore()
        {
            Console.Clear();
            Console.WriteLine("Highscore");
            Console.ReadLine();
        }
        /// <summary>
        /// To display "About"
        /// </summary>
        public static void About()
        {
            Console.Clear();
            Console.WriteLine("About");
            Console.ReadLine();
        }
        /// <summary>
        /// Change the color of chosen option
        /// </summary>
        public static void Activate()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Reset the color
        /// </summary>
        public static void Desactivate()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
