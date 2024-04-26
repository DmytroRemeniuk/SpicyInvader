using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

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
        SoundPlayer switchMenu = new SoundPlayer();
        SoundPlayer selectMenu = new SoundPlayer();

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
                DisplayMenu();

                _menuChoice = 1;

                do
                {
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (_menuChoice > 1)
                            {
                                _menuChoice--;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if (_menuChoice < 5)
                            {
                                _menuChoice++;
                            }
                            break;
                        case ConsoleKey.Enter:
                            _goon = true;
                            break;
                    }
                    Console.Clear();
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
            Console.Clear();
            Console.WriteLine("Highscore");
            Console.ReadLine();
        }
        /// <summary>
        /// To display "About"
        /// </summary>
        private void About()
        {
            Console.Clear();
            Console.WriteLine("About");
            Console.ReadLine();
        }
        /// <summary>
        /// Dispaly the title and the menu
        /// </summary>
        private void DisplayMenu()
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
        }
    }
}
