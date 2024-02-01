﻿/*
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

namespace SpicyInvader
{
    internal class Program
    {
        const int ENEMY_QUANTITY_X = 10;
        const int ENEMY_QUANTITY_Y = 7;
        [STAThread]
        static void Main(string[] args)
        {
            #region Variables
            string menuChoice;//to recover the user's input
            int x;//to recover the spaceship's position
            int oldX = 0;//stock of x
            #endregion

            #region Constants
            //for the size of window
            const int WINDOW_WIDTH = 125;
            const int WINDOW_HEIGHT = 55;
            //start position
            const int START_X = 20;
            const int START_Y = 50;
            //limitation
            const int MAX_Y = 120;
            const int MIN_Y = 0;
            //spaceship
            const string SPACESHIP = "«=ˆ=»";
            //for enemies
            
            const int ENEMY_START_X = 35;
            const int ENEMY_START_Y = 10;
            #endregion

            Enemy[,] enemyTable = new Enemy[ENEMY_QUANTITY_X, ENEMY_QUANTITY_Y];
            Enemy enemy = new Enemy(ENEMY_START_X, ENEMY_START_Y);
            Blast blast = new Blast(0, -1);
            

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

            //game method
            void Game()
            {
                Console.Clear();
                //display in a specific location
                Console.SetCursorPosition(START_X, START_Y);
                x = START_X;//give the variable a start point
                Console.WriteLine(SPACESHIP);//display a spaceship

                //create the enemies
                for(int i = 0; i < ENEMY_QUANTITY_X; i++)
                {
                    for(int j = 0; j < ENEMY_QUANTITY_Y; j++)
                    {
                        enemy = new Enemy(positionX : ENEMY_START_X + i*6, positionY : ENEMY_START_Y + j*2);
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
                        MovementControl();

                        if (Keyboard.IsKeyDown(Key.Space))
                        {
                            blast = new Blast(positionX: x + 2, positionY: START_Y - 1);

                            //shooting
                            while (blast.PositionY > 0)
                            {
                                if (blast.PositionY < START_Y - 1)
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
                                MovementControl();
                                Move();

                                //wait for 25 milliseconds between movement/speed
                                Thread.Sleep(25);
                            }
                            
                        }
                        Move();
                        Thread.Sleep(25);
                    }


                    //method that moves the spaceship
                    void Move()
                    {
                        Console.SetCursorPosition(oldX, START_Y);
                        Console.Write(" ");
                        if(oldX > MIN_Y)
                        {
                            Console.SetCursorPosition(oldX - 1, START_Y);
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(oldX + SPACESHIP.Length - 1, START_Y);
                        Console.Write(" ");
                        if (oldX > MIN_Y && oldX < MAX_Y)
                        {
                            Console.SetCursorPosition(oldX + SPACESHIP.Length, START_Y);
                            Console.Write(" ");
                        }
                        Console.SetCursorPosition(x, START_Y);
                        Console.Write(SPACESHIP); 
                    }
                    //method for recovering of the pressed keys and change the coordinates
                    void MovementControl()
                    {
                        if (Keyboard.IsKeyDown(Key.Left) || Keyboard.IsKeyDown(Key.A))
                        {
                            if (x > MIN_Y)
                            {
                                oldX = x;
                                x--;
                            }
                        }

                        if (Keyboard.IsKeyDown(Key.Right) || Keyboard.IsKeyDown(Key.D))
                        {
                            if (x < MAX_Y)
                            {
                                oldX = x;
                                x++;
                            }
                        }
                    }
                } while(true);
            }
        }

        
    }
}

