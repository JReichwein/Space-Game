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
    class locationArrow
    {
        Texture2D arrow;
        public Vector2 arrowPos;
        Vector2 vector = Vector2.Zero;
        float ang = 0;
        Vector2 origin;
        int x = 0;
        int y = 0;
        int dist;

        public locationArrow(Texture2D text, Vector2 middleOfHub)
        {
            arrow = text;
            vector = middleOfHub;
            origin = new Vector2(arrow.Width / 2, arrow.Height / 2);
        }
        public void update(KeyboardState keyboard, Vector2 camPos, Vector2 localPos)
        {
            //355, -210
            arrowPos = new Vector2(camPos.X - 355, camPos.Y - 205);
            Vector2 dir = Vector2.Zero - localPos;
            dir.Normalize();
            ang = lookAt(dir);

            dist = (int)Vector2.Distance(localPos, new Vector2(0, 0));

            /*
            if (keyboard.IsKeyDown(Keys.Right))
                x++;
            if (keyboard.IsKeyDown(Keys.Left))
                x--;
            if (keyboard.IsKeyDown(Keys.Down))
                y++;
            if (keyboard.IsKeyDown(Keys.Up))
                y--;
            */
        }
        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(arrow, arrowPos, null, Color.LightGreen, ang + 90, origin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, (dist / 100).ToString() + "m", new Vector2(arrowPos.X - 30, arrowPos.Y + 40), Color.LightGreen);
        }
        float lookAt(Vector2 thingToLookAt)
        {
            return (float)Math.Atan2(thingToLookAt.X, -thingToLookAt.Y);
        }
    }
}

