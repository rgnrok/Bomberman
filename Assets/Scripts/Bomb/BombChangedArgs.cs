using System;

namespace Bomberman
{
    public class BombChangedArgs : EventArgs
    {
        public int bombsCount;

        public BombChangedArgs(int count) {
            bombsCount = count;
        }
    }
}
