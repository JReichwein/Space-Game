﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceGame
{
    class Missile
    {
        Texture2D missileTex;
        Rectangle missileRect;
        Vector2 missileCenter;
        double heading;
        double posX, posY;

        public Missile(ContentManager c, Vector2 location, double heading, Vector2 origin)
        {
            ContentManager content = c;
            missileTex = content.Load<Texture2D>("Missile");
            posX = location.X - 25;
            posY = location.Y - 25;
            missileRect = new Rectangle((int)posX, (int)posY, 50, 50);
            missileCenter = new Vector2((float)posX + missileRect.Width,
                                        (float)posY + missileRect.Height);
            this.heading = heading;
        }

        public void Update(GameTime gameTime)
        {
            posX += 0.1 * Math.Sin(heading);
            posY += 0.1 * -Math.Cos(heading);
            missileCenter = new Vector2((int)posX + missileRect.Width / 2,
                                        (int)posY + missileRect.Height / 2);
        }

        private float vectorToAngle(Vector2 angleVector)
        {
            return (float)Math.Atan2(angleVector.X, -angleVector.Y);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(missileTex, missileCenter, null, Color.White,
                (float)heading,
                new Vector2(0,0), 0.01f,
                SpriteEffects.None, 0);
        }

        public Vector2 Location
        {
            get
            {
                return missileCenter;
            }
        }
    }
}
