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
        private Texture2D hubText, radiusText, backgroundText;
        private Rectangle hubRadius, radiusHitbox;
        private Color radiusColor, menuColor;
        private bool menuOpened;
        private string[] enterPrompt;

        public Hub(ContentManager manager)
        {
            hubText = manager.Load<Texture2D>("Hub");
            radiusText = manager.Load<Texture2D>("Hub Radius");
            backgroundText = manager.Load<Texture2D>("HubBackground");
            hubOrigin = new Vector2(hubText.Width / 2, hubText.Height / 2);
            radiusOrigin = new Vector2(radiusText.Width / 2, radiusText.Height / 2);
            hub_pos = new Vector2(0, 0);
            hubRadius = new Rectangle((int)hub_pos.X, (int)hub_pos.Y, (int)(hubText.Width * 1.5), (int)(hubText.Height * 1.5));
            radiusHitbox = new Rectangle(hubRadius.X - (hubRadius.Width / 2), hubRadius.Y - (hubRadius.Height / 2), hubRadius.Width, hubRadius.Height);
            radiusColor = new Color(0, 50, 0, 50);
            menuColor = new Color(0, 0, 0, 0);
            menuOpened = false;
            enterPrompt = new String[2] { "Press X to enter the Hub", "Press E to enter the Hub" };
        }

        public void Update(GameTime gameTime, GamePadState pad, Player player)
        {
            if (isWithinRadius(player.getRectangle()) && pad.Buttons.X == ButtonState.Pressed && !menuOpened)
                menuOpened = true;
            else if (pad.Buttons.Y == ButtonState.Pressed && menuOpened && menuColor.A == 255)
                menuOpened = false;

            if (menuOpened && menuColor.A != 255)
            {
                menuColor.R += 5;
                menuColor.G += 5;
                menuColor.B += 5;
                menuColor.A += 5;
            }
            else if (!menuOpened && menuColor.A != 0)
            {
                menuColor.R -= 5;
                menuColor.G -= 5;
                menuColor.B -= 5;
                menuColor.A -= 5;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hubText, hub_pos, null, Color.White, 0f, hubOrigin, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
        }

        public void DrawMenu(SpriteBatch spriteBatch, SpriteFont menuFont, Camera2D camera)
        {
            if(menuOpened)
            {
                Rectangle bounds = camera.getBounds();
                int camX = (int)(camera.Location.X - camera.getBounds().X / 2);
                int camY = (int)(camera.Location.Y - camera.getBounds().Y / 2);
                spriteBatch.Draw(backgroundText, new Rectangle(camX - bounds.Width / 2, camY - bounds.Height / 2, bounds.Width, bounds.Height), menuColor);
            }
            else
            {
                spriteBatch.DrawString(menuFont, enterPrompt[0], new Vector2(-menuFont.MeasureString(enterPrompt[0]).X / 2, -menuFont.MeasureString(enterPrompt[0]).Y / 2), Color.White);
            }
        }

        public bool isWithinRadius(Rectangle playerRect)
        {
            return playerRect.Intersects(radiusHitbox);
        }

        public bool isOnCamera(Camera2D camera)
        {
            Rectangle bounds = camera.getBounds();
            int camX = (int)(camera.Location.X - camera.getBounds().X / 2);
            int camY = (int)(camera.Location.Y - camera.getBounds().Y / 2);
            return camX <= hubRadius.X + hubRadius.Width && camX >= hubRadius.X - hubRadius.Width &&
                camY <= hubRadius.Y + hubRadius.Height && camY >= hubRadius.Y - hubRadius.Height;
        }

        public bool isInHub()
        {
            return menuOpened;
        }
    }
}
