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
    public class Asteroid 
    {
        private Vector2 position;
        private Rectangle bounds;

        private bool isMining = false;
        private bool mined = false;
        private bool showHint = false;

        private int rawResource = 10;
        private int difficulty = 10;
        private int amountMineLeft = 1000;

        private Vector2 radiusOrigin;
        private Texture2D radiusText;
        private SpriteFont menuFont;

        public bool shouldDraw = true;
        //private Color radiusColor = new Color(0, 50, 0, 50);

        private Rectangle asteroidRadius;

        string[] enterPrompt = new String[2] { "Hold A to mine", "Hold E to mine" };

        public Asteroid(Texture2D texture, Vector2 position, SpriteFont menuFont) 
        {
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, 20, 20);

            asteroidRadius = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * 2), (int)(texture.Height * 2));

            //radiusText = c.Load<Texture2D>("Hub Radius");
            radiusOrigin = new Vector2(50);

            radiusText = null;

            this.menuFont = menuFont;
            texture = null;

        }

        public bool mine()
        {
            Console.WriteLine();
            Console.WriteLine("Mining Asteroid");
            amountMineLeft -= difficulty;
            Console.WriteLine("Mining: " + amountMineLeft);
            Console.WriteLine("Done.");
            return (amountMineLeft <= 0);
        }


        public void Draw(Texture2D texture, GameTime gameTime, SpriteBatch sb, Player p)
        {
            if (!mined)
            {
                sb.Draw(texture, bounds, Color.LightPink);
                //sb.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
            }
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

        public bool Mined
        {
            set
            {
                mined = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public bool isWithinRadius(Rectangle playerRect)
        {
            return playerRect.Intersects(asteroidRadius);
        }

        public bool isOnCamera(Camera2D camera)
        {
            Rectangle bounds = camera.getBounds();
            int camX = (int)(camera.Location.X - camera.getBounds().X / 2);
            int camY = (int)(camera.Location.Y - camera.getBounds().Y / 2);
            return camX <= asteroidRadius.X + asteroidRadius.Width && camX >= asteroidRadius.X - asteroidRadius.Width &&
                camY <= asteroidRadius.Y + asteroidRadius.Height && camY >= asteroidRadius.Y - asteroidRadius.Height;
        }
    }
}
