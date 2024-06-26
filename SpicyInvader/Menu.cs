﻿using System;
using System.Media;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SpicyInvader
{
    class Menu
    {
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

        //variables
        int _menuChoice;//to recover the user's input
        bool _goon = false;
        Game _spicy;
        SoundPlayer switchMenu = new SoundPlayer("..\\..\\Sounds\\switchMenu.wav");
        SoundPlayer selectMenu = new SoundPlayer("..\\..\\Sounds\\selectMenu.wav");

        string[] menuTable = new string[] { "",
                                                "Start the game",
                                                "    Options   ",
                                                "   Highscore  ",
                                                "     About    ",
                                                "     Quit     "};

        public Menu(Game spicy)
        {
            _spicy = spicy;
        }
        /// <summary>
        /// Main menu of the game
        /// </summary>
        public void MainMenu()
        {
            //menu and a loop to continue the game
            do
            {
                Console.Clear();

                _menuChoice = 1;

                DisplayMenu();

                do
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            switchMenu.Play();
                            if (_menuChoice > 1)
                            {
                                _menuChoice--;
                            }
                            else
                            {
                                _menuChoice = 5;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            switchMenu.Play();
                            if (_menuChoice < 5)
                            {
                                _menuChoice++;
                            }
                            else
                            {
                                _menuChoice = 1;
                            }
                            break;
                        case ConsoleKey.Enter:
                            selectMenu.Play();
                            _goon = true;
                            break;
                    }
                    Console.Clear();
                    DisplayMenu();
                } while (!_goon);//quit the loop after pressing "Enter"

                _goon = false;

                //verify the choice and pass to the chosen stage
                switch (_menuChoice)
                {
                    case 1:
                        _spicy.NewGame();
                        break;
                    case 2:
                        Options();
                        break;
                    case 3:
                        Highscore();
                        break;
                    case 4:
                        About();
                        break;
                }
            } while (_menuChoice != 5);//quit
        }
        /// <summary>
        /// Change the color of chosen option
        /// </summary>
        private void Activate()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        /// <summary>
        /// Reset the color
        /// </summary>
        private void Desactivate()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// To display the options
        /// </summary>
        private void Options()
        {
            Console.Clear();
            Console.WriteLine("Options");
            Console.ReadLine();
        }
        /// <summary>
        /// To display the highscores
        /// </summary>
        private void Highscore()
        {
            string[] allText;
            int cursorY = Constants.MAX_Y / 2 - 10;
            const int CURSOR_X = Constants.MAX_X / 2;


            Console.Clear();

            Console.SetCursorPosition(CURSOR_X - 2, cursorY);
            Console.WriteLine("Highscores:");
            cursorY += 2;

            foreach (string file in Directory.GetFiles(Constants.HIGHSCORE))
            {
                allText = File.ReadAllLines(file).ToArray();
                Console.SetCursorPosition(CURSOR_X, cursorY);
                Console.Write(allText[0] + ": " + allText[1]);
                cursorY++;
            }
            Activate();
            cursorY += 10;
            Console.SetCursorPosition(CURSOR_X - 1, cursorY);
            Console.WriteLine("Continuer");
            Desactivate();
            Console.ReadLine();
        }
        /// <summary>
        /// To display "About"
        /// </summary>
        private void About()
        {
            Console.Clear();
            Console.WriteLine("The goal is to kill the most enemies possible. You can change the mode between simple and difficult in the options.\n" +
                              "<- -> or [A] [D] to move\n" +
                              "[SPACE] to shoot\n" +
                              "[ESC] to finish the game");
            Console.ReadLine();
        }
        /// <summary>
        /// Dispaly the title and the menu
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine(TITLE);
            for (int i = 1; i < menuTable.Length; i++)
            {
                if (_menuChoice == i)
                {
                    Activate();
                }
                Console.SetCursorPosition(55, 20 + i);
                Console.WriteLine(menuTable[i]);
                Desactivate();
            }
        }
    }
}
