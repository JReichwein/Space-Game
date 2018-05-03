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
    class Asteroid
    {
        private Texture2D texture;
        public Vector2 position;
        private Rectangle bounds;

        private bool isMining = false;

        private int rawResource = 10;
        private int difficulty = 10;
        private int amountMineLeft = 100;
        private int mineRadius = 100;
        
        private Rectangle hubRadius, radiusHitbox;

        public Asteroid(ContentManager c, Vector2 position)
        {
            texture = c.Load<Texture2D>("Asteroid1");
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);

            hubRadius = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * 1.5), (int)(texture.Height * 1.5));
            radiusHitbox = new Rectangle(hubRadius.X - (hubRadius.Width / 2), hubRadius.Y - (hubRadius.Height / 2), hubRadius.Width, hubRadius.Height);
        }

        public bool mine()
        {
            amountMineLeft -= difficulty;
            return amountMineLeft <= 0;
        }

        public void Update(GameTime gameTime)
        {

        }

        public bool isWithinRadius(Rectangle playerRect)
        {
            return playerRect.Intersects(radiusHitbox);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(texture, bounds, Color.LightPink);
            //sb.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
        }

        public Rectangle Rectangle
        {
            get
            {
                return bounds;
            }
        }
    }
}
