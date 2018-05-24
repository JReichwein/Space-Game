using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    class Collidable
    {
        public float x;
        public float y;
        public string tag;

        public Collidable(int x, int y, string tag)
        {
            this.x = x;
            this.y = y;
            this.tag = tag;
        }

        public Rectangle getRect()
        {
            throw new NotImplementedException();
        }
    }
}