/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/
using System;
using System.Runtime.InteropServices;

namespace SpicyInvader
{
    internal class Program
    {
        #region Block the resizing of console (const, import dll and methods)
        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;
        private const int WINDOW_WIDTH = 125;
        private const int WINDOW_HEIGHT = 55;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        #endregion //source - https://stackoverflow.com/questions/38426338/c-sharp-console-disable-resize

        [STAThread]
        static void Main()
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

            //set the size of the console
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            //disable scrolling
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            //hide the cursor
            Console.CursorVisible = false;

            Game spicy = new Game();
            Menu newMenu = new Menu(spicy: spicy);

            newMenu.MainMenu();
        }
    }
}
