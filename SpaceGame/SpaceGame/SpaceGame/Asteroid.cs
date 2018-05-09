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
        private bool showHint = false;

        private int rawResource = 10;
        private int difficulty = 10;
        private int amountMineLeft = 100;
        private int mineRadius = 100;

        private Vector2 radiusOrigin;
        private Texture2D radiusText;
        private SpriteFont menuFont;
        private Color radiusColor = new Color(0, 50, 0, 50);

        private Rectangle hubRadius;

        string[] enterPrompt = new String[2] { "Press A to mine", "Press E to mine" };

        public Asteroid(ContentManager c, Vector2 position)
        {
            texture = c.Load<Texture2D>("Asteroid1");
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);

            hubRadius = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * 2), (int)(texture.Height * 2));

            radiusText = c.Load<Texture2D>("Hub Radius");
            radiusOrigin = new Vector2(radiusText.Width / 2, radiusText.Height / 2);

            menuFont = c.Load<SpriteFont>("MenuFont");
        }

        public bool mine()
        {
            amountMineLeft -= difficulty;
            Console.WriteLine("Mining: " + amountMineLeft);
            return true;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch sb, Player p)
        {
            sb.Draw(texture, bounds, Color.LightPink);
            sb.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
            if (showHint)
                sb.DrawString(menuFont, enterPrompt[0], p.player_pos - new Vector2(menuFont.MeasureString(enterPrompt[0]).X / 2, (menuFont.MeasureString(enterPrompt[0]).Y / 2) - 100), Color.White);
        }

        public Rectangle Rectangle
        {
            get
            {
                return bounds;
            }
        }

        public bool IsMining
        {
            get
            {
                return isMining;
            }
            set
            {
                isMining = value;
            }
        }

        public bool Hint
        {
            set
            {
                showHint = value;
            }
        }

        public int RawResources
        {
            get
            {
                return rawResource;
            }
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
