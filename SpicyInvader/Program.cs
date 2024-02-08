/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console
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
        private const int ENEMY_QUANTITY_X = 10;
        private const int ENEMY_QUANTITY_Y = 7;
        //time of await of movement
        private const int SLEEP = 10;
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
            int oldX;//stock of x

            Enemy[,] enemyTable = new Enemy[ENEMY_QUANTITY_X, ENEMY_QUANTITY_Y];
            Enemy enemy = new Enemy(ENEMY_START_X, ENEMY_START_Y);
            Blast blast = new Blast(0, -1);


            Console.Clear();
            //create a spaceship
            Spaceship spaceship = new Spaceship();
            //display in a specific location
            Console.SetCursorPosition(spaceship.PositionX, spaceship.PositionY);
            //display a spaceship
            Console.WriteLine(spaceship.Display);

            //create the enemies
            for (int i = 0; i < ENEMY_QUANTITY_X; i++)
            {
                for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                {
                    enemy = new Enemy(positionX: ENEMY_START_X + i * 6, positionY: ENEMY_START_Y + j * 2);
                    Console.SetCursorPosition(enemy.PositionX, enemy.PositionY);
                    Console.Write(enemy.Display);
                    enemyTable[i, j] = enemy;
                }
            }

            do
            {
                //moving
                while (true)
                {
                    oldX = spaceship.MovementControl(0);

                    if (Keyboard.IsKeyDown(Key.Space))
                    {
                        blast = new Blast(positionX: spaceship.PositionX + 2, positionY: spaceship.PositionY - 1);

                        //shooting
                        while (blast.PositionY > 0)
                        {
                            if (blast.PositionY < spaceship.PositionY - 1)
                            {
                                Console.SetCursorPosition(blast.PositionX, blast.PositionY);
                                Console.Write(" ");
                            }
                            blast.PositionY--;
                            Console.SetCursorPosition(blast.PositionX, blast.PositionY);
                            Console.Write(blast.Display);
                            if (blast.PositionY == 0)
                            {
                                Console.SetCursorPosition(blast.PositionX, blast.PositionY);
                                Console.Write(" ");
                            }

                            oldX = spaceship.MovementControl(0);
                            spaceship.Move(oldX);

                            for (int i = 0; i < ENEMY_QUANTITY_X; i++)
                            {
                                for (int j = 0; j < ENEMY_QUANTITY_Y; j++)
                                {
                                    if (enemyTable[i, j].Display != "     " && blast.PositionY == enemyTable[i, j].PositionY && blast.PositionX == enemyTable[i, j].PositionX || enemyTable[i, j].Display != "     " && blast.PositionY == enemyTable[i, j].PositionY && blast.PositionX == enemyTable[i, j].PositionX + 1 || enemyTable[i, j].Display != "     " && blast.PositionY == enemyTable[i, j].PositionY && enemyTable[i, j].Display != "     " && blast.PositionX == enemyTable[i, j].PositionX + 2 || enemyTable[i, j].Display != "     " && blast.PositionY == enemyTable[i, j].PositionY && blast.PositionX == enemyTable[i, j].PositionX + 3 || enemyTable[i, j].Display != "     " && blast.PositionY == enemyTable[i, j].PositionY && blast.PositionX == enemyTable[i, j].PositionX + 4)
                                    {
                                        enemyTable[i, j].Display = "     ";
                                        Console.SetCursorPosition(enemyTable[i, j].PositionX, enemyTable[i, j].PositionY);
                                        Console.Write(enemyTable[i, j].Display);
                                        blast.PositionY = 1;
                                    }
                                }
                            }
                            //wait for *SLEEP* milliseconds between movement/speed
                            Thread.Sleep(SLEEP);
                        }
                    }
                    spaceship.Move(oldX);
                    Thread.Sleep(SLEEP);
                }
            } while (true);
        }
    }
}

