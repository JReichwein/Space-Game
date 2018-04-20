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

namespace SpaceGame
{
    class Missile
    {
        Texture2D missileTex;
        Rectangle missileRect;
        Vector2 missileCenter;
        double heading;
        double posX, posY;

        public Missile(Texture2D texture, Vector2 location, double heading)
        {
            missileTex = texture;
            posX = location.X;
            posY = location.Y;
            missileRect = new Rectangle((int)posX, (int)posY, 28, 50);
            missileCenter = new Vector2(missileRect.X + missileRect.Width / 2,
                                        missileRect.Y + missileRect.Height / 2);
            this.heading = heading;
        }

        public void Update(GameTime gameTime)
        {
            posX += 5 * Math.Cos(heading - 90);
            posY += 5 * Math.Sin(heading - 90);
            missileCenter = new Vector2((int)posX + missileRect.Width / 2,
                                        (int)posY + missileRect.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(missileTex, missileCenter, null, Color.White,
                (float)heading,
                new Vector2(missileTex.Width / 2, missileTex.Height / 2), 0.2f,
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
