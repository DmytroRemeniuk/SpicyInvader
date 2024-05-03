/*
    ETML
    Autor         : Dmytro Remeniuk
    Date          : 18.01.2024
    Description   : the game Space Invader (Spicy Invader) on the console Windows
*/

namespace SpicyInvader
{
    internal class Barricade : GameObject
    {
        public Barricade(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
            _display = "▓";
            _lives = 2;
        }

        #region Getters&Setters
        public int Lives
        {
            get { return _lives; }
            set { _lives = value; }
        }
        #endregion

        public static Barricade[,] Create()
        {
            Barricade[,] barricades = new Barricade[Constants.BARR_QUANTITY_Y, Constants.BARR_QUANTITY_X];

            //create the barricades
            for (int i = 0; i < Constants.BARR_QUANTITY_Y; i++)
            {
                for (int j = 0; j < Constants.BARR_QUANTITY_X; j++)
                {
                    if (j < 8 || j > 40 && j < 49 || j > 81)
                    {
                        barricades[i, j] = new Barricade(Constants.BARR_X + j, Constants.BARR_Y + i);
                        barricades[i, j].Write();
                    }
                }
            }

            return barricades;
        }
    }
}
