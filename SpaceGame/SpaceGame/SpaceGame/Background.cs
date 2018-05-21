using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace SpaceGame
{
    class Background
    {

        Texture2D text;
        public int hTileCount;
        public int vTileCount;
        public Vector2 start;
        public Vector2 oldstart;
        Rectangle[,] bgs;
        bool init = false;
        Vector2 parallaxAngle = Vector2.Zero;
        //379wx391h  < this is wrong but the code works so w/e


        public Background(Texture2D _text, int envWidth, int envHeight)
        {
            text = _text;
            hTileCount = (int)(Math.Round((double)envWidth / text.Width) + 3);
            vTileCount = (int)(Math.Round((double)envHeight / text.Height) + 3);
            start = Vector2.Zero;
            oldstart = Vector2.Zero;
            bgs = new Rectangle[hTileCount, vTileCount];

        }
        //

        public void update(Rectangle camRect, Player local)
        {
            //initialize start point so you don't see black in spawn
            if (!init)
            {
                start.X = camRect.X - (hTileCount / 2) * text.Width - 400;
                start.Y = camRect.Y - (vTileCount / 2) * text.Height - 400;
                init = true;
            }

            char side = checkbuffer(camRect, bgs); //call func for checking boundaries

            if (local.isMoving) // 5 min parallax code lol
                start = parallax(angleToVector((float)local.ang), start);

            //decide if we should move the backgrounds
            switch (side)
            {
                case 'l':
                    start.X -= text.Width;
                    //Console.WriteLine("left");
                    break;
                case 'r':
                    start.X += text.Width;
                    //Console.WriteLine("right");
                    break;
                case 'u':
                    start.Y += text.Height;
                    //Console.WriteLine("up");
                    break;
                case 'd':
                    start.Y -= text.Height;
                    //Console.WriteLine("down");
                    break;
                case 'x':
                    break;
                default:
                    break;
            }

            // update rectangles every update because im dumb and can't optimize code
            for (int i = 0; i < hTileCount; i++)
                for (int j = 0; j < vTileCount; j++)
                {
                    bgs[i, j] = new Rectangle(
                        (int)start.X + (i * text.Width),
                        (int)start.Y + (j * text.Height),
                        text.Width,
                        text.Height
                        );
                }
        }


        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < hTileCount; i++) //nested for-loop for drawing array
            {
                for (int j = 0; j < vTileCount; j++)
                {
                    spriteBatch.Draw(text,
                        bgs[i, j],
                        Color.White);
                }
            }
        }
        public char checkbuffer(Rectangle player, Rectangle[,] bgs)
        {
            // up
            for (int i = 0; i < vTileCount; i++)
            {
                if (player.Intersects(bgs[i, (vTileCount - 1)]))
                {
                    return 'u';
                }
            }
            // down
            for (int i = 0; i < vTileCount; i++)
            {
                if (player.Intersects(bgs[i, 1]))
                {
                    return 'd';
                }
            }
            // left
            for (int i = 0; i < hTileCount; i++)
            {
                if (player.Intersects(bgs[1, i]))
                {
                    return 'l';
                }
            }
            // right
            for (int i = 0; i < hTileCount; i++)
            {
                if (player.Intersects(bgs[(hTileCount - 1), i]))
                {
                    return 'r';
                }
            }
            return 'x';
        }

        public Vector2 parallax(Vector2 vel, Vector2 start)
        {
            Vector2 newstart = (vel * 2.3f) + start;
            return newstart;
        }

        Vector2 angleToVector(float angle) //pasta'd anglevector
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }
    }
}