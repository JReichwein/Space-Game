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
    class Hub
    {
        private Vector2 hubOrigin, radiusOrigin, hub_pos;
        private Texture2D hubText, radiusText;
        private Rectangle hubRadius;
        private Color radiusColor;

        public Hub(ContentManager manager)
        {
            hubText = manager.Load<Texture2D>("Hub");
            radiusText = manager.Load<Texture2D>("Hub Radius");
            hubOrigin = new Vector2(hubText.Width / 2, hubText.Height / 2);
            radiusOrigin = new Vector2(radiusText.Width / 2, radiusText.Height / 2);
            hub_pos = new Vector2(0, 0);
            hubRadius = new Rectangle((int)hub_pos.X, (int)hub_pos.Y, (int)(hubText.Width * 1.5), (int)(hubText.Height * 1.5));
            radiusColor = new Color(0, 50, 0, 50);
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hubText, hub_pos, null, Color.White, 0f, hubOrigin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
        }

        public bool isWithinRadius(Rectangle playerRect)
        {
            return playerRect.Intersects(hubRadius);
        }

        public bool isOnCamera(Camera2D camera)
        {
            Rectangle bounds = camera.getBounds();
            int camX = (int)(camera.Location.X - camera.getBounds().X / 2);
            int camY = (int)(camera.Location.Y - camera.getBounds().Y / 2);
            return camX <= hubRadius.X + hubRadius.Width && camX >= hubRadius.X - hubRadius.Width &&
                camY <= hubRadius.Y + hubRadius.Height && camY >= hubRadius.Y - hubRadius.Height;
        }
    }
}
