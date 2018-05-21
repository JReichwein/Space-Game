using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    class Collidable
    {

        public struct vec2
        {
            public float x;
            public float y;
            public int width;
            public int height;

            public vec2(float x, float y, int width, int height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
            }
        }



        public bool isColliding;
        vec2 rect;
        public Object colliding;
        public Object reference;


        public Collidable(vec2 vec)
        {
            isColliding = false;
            this.rect = vec;
            
        }

        public void setRef(object x)
        {
            this.reference = x;
        }


        public void setRect(vec2 rect)
        {
            this.rect = rect;
        }

        public Rectangle getRect()
        {
            return new Rectangle((int)rect.x, (int)rect.y, rect.width, rect.height);
        }

        public void exitCollision()
        {
            this.colliding = null;
            this.isColliding = false;
        }


        public void onCollision(Collidable c)
        {
            if (getRect().Intersects(c.getRect()))
            {
                this.colliding = c.reference;
                this.isColliding = true;

                c.colliding = reference;
                c.isColliding = true;


                Console.WriteLine("collision");
            }
        }


    }
}
