using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SpicyInvader
{
    internal class GameObject
    {
        //common attributes
        #region Variables
        protected int _positionX = 0;
        protected int _positionY = 0;
        protected string _display = "";
        protected int _lives = 0;
        #endregion

        #region Getters&Setters

        public string Display
        {
            get { return _display; }
            set { _display = value; }
        }

        public int PositionX
        {
            get { return _positionX; }
            set { _positionX = value; }
        }

        public int PositionY
        {
            get { return _positionY; }
            set { _positionY = value; }
        }
        #endregion

        #region Methods
        //common
        /// <summary>
        /// Write the object's display in the right place
        /// </summary>
        public void Write()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(this.Display);
        }
        /// <summary>
        /// Erase the object on the previous position
        /// </summary>
        public void Erase()
        {
            Console.SetCursorPosition(this.PositionX, this.PositionY);
            Console.Write(" ");
        }
        #endregion
    }
}
