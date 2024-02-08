using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpicyInvader
{
    internal class Blast
    {
        private const string DISPLAY = "|";
        private int _positionX = 0;
        private int _positionY = 0;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public Blast(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

        #region Getters&Setters
        public string Display
        {
            get { return DISPLAY; }
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

        #endregion
    }
}
