using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpicyInvader
{
    internal class Enemy
    {
        private string _display = "(\\!/)";
        private int _positionX = 0;
        private int _positionY = 0;

        public Enemy(int positionX, int positionY)
        {
            _positionX = positionX;
            _positionY = positionY;
        }

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
    }
}
