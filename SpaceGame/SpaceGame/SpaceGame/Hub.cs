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
    class HubButton
    {
        private Rectangle buttonRect;
        private Texture2D buttonText;
        private Color buttonColor, textColor;
        private String text;
        private Vector2 textSize;
        private MouseState oldMouse;
        private SpriteFont buttonFont;
        private int requiredResources;

        public HubButton(Vector2 position, String text, ContentManager manager, int requiredResources)
        {
            buttonText = manager.Load<Texture2D>("Button");
            buttonFont = manager.Load<SpriteFont>("ButtonFont");
            this.text = text + " (" + requiredResources + ")";
            textSize = new Vector2((int)(buttonFont.MeasureString(this.text).X), (int)(buttonFont.MeasureString(this.text).Y));
            buttonRect = new Rectangle((int)position.X - (int)textSize.X / 2 - 18, (int)position.Y - (int)textSize.Y / 2 - 7, (int)textSize.X + 36, (int)textSize.Y + 14);
            buttonColor = new Color(1, 1, 1, 0);
            textColor = Color.Black;
            oldMouse = Mouse.GetState();
            this.requiredResources = requiredResources;
        }

        public void Update(GameTime gameTime, Camera2D camera, Color menuColor)
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            mousePosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.TransformMatrix()));
            
            if (isTouchingButton(new Vector2(mousePosition.X, mousePosition.Y )))
            {
                buttonColor = Color.Red;
                if(mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                {
                    Console.Beep();
                }
            }
            else
            {
                buttonColor = Color.White;
            }

            if (buttonColor.G > 0)
            {
                buttonColor = menuColor;
            }
            else
            {
                buttonColor.R = menuColor.R;
                buttonColor.A = menuColor.A;
            }
            textColor.A = menuColor.A;

            oldMouse = mouse;
        }

        private bool isTouchingButton(Vector2 position)
        {
            // add 300 to the position of each component for proper detections.
            return new Rectangle((int)position.X, (int)position.Y, 1, 1).Intersects(buttonRect);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            spriteBatch.Draw(buttonText, buttonRect, buttonColor);
            spriteBatch.DrawString(buttonFont, text, new Vector2(buttonRect.X + (buttonRect.Width - buttonFont.MeasureString(text).X) / 2, buttonRect.Y + (buttonRect.Height - buttonFont.MeasureString(text).Y) / 2), textColor);
        }

        public bool isButton(String name)
        {
            return name.Equals(text);
        }
    };

    class Hub
    {
        private Vector2 hubOrigin, radiusOrigin, hub_pos;
        private Texture2D hubText, radiusText, backgroundText;
        private Rectangle hubRadius, radiusHitbox;
        private Color radiusColor, menuColor;
        private bool menuOpened;
        private string[] enterPrompt;
        private List<HubButton> buttons;
        private SpriteFont menuFont, smallMenuFont;

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
            buttons = new List<HubButton>();
            menuFont = manager.Load<SpriteFont>("MenuFont");
            smallMenuFont = manager.Load<SpriteFont>("SmallMenuFont");
        }

        public void Update(GameTime gameTime, GamePadState pad, Player player, Camera2D camera, ContentManager manager, Player p1)
        {
            if (isWithinRadius(player.getRectangle()) && pad.Buttons.X == ButtonState.Pressed && !menuOpened)
            {
                menuOpened = true;
                Rectangle bounds = camera.getBounds();
                Vector2 center = new Vector2(camera.Location.X, camera.Location.Y);
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y - camera.getBounds().Height / 3.5)), "UPGRADE SPEED", manager, 0));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y - camera.getBounds().Height / 7)), "UPGRADE ARMOR", manager, 0));
                buttons.Add(new HubButton(new Vector2(center.X, center.Y), "UPGRADE DAMAGE", manager, 0));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y + camera.getBounds().Height / 7)), "UPGRADE RATE OF FIRE", manager, 0));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y + camera.getBounds().Height / 3.5)), "UPGRADE STORAGE", manager, 0));
            }
            else if (pad.Buttons.Y == ButtonState.Pressed && menuOpened && menuColor.A == 255)
            {
                menuOpened = false;
            }

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

            if (pad.Buttons.B == ButtonState.Pressed && p1.getRawResources() > 0)
            {
                p1.setResources(p1.getRawResources() + p1.getResources());
                p1.setRawResources(0);
            }

            foreach(HubButton button in buttons)
            {
                button.Update(gameTime, camera, menuColor);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hubText, hub_pos, null, Color.White, 0f, hubOrigin, 1.0f, SpriteEffects.None, 0f);
        }

        public void DrawRadius(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(radiusText, hubRadius, null, radiusColor, 0f, radiusOrigin, SpriteEffects.None, 0f);
        }
        
        public void DrawMenu(SpriteBatch spriteBatch, Camera2D camera, int resources, int rawResources)
        {
            if(menuColor.A != 0)
            {
                Rectangle bounds = camera.getBounds();
                int camX = (int)(camera.Location.X - camera.getBounds().X / 2);
                int camY = (int)(camera.Location.Y - camera.getBounds().Y / 2);
                spriteBatch.Draw(backgroundText, new Rectangle(camX - bounds.Width / 2, camY - bounds.Height / 2, bounds.Width, bounds.Height), menuColor);
                // Resource Count
                spriteBatch.DrawString(menuFont, "Resources: " + resources, new Vector2(camX - menuFont.MeasureString("Resources: " + resources).X / 2 - 3, (int)(camY - bounds.Height / 2.5 - menuFont.MeasureString("Resources: " + resources).Y / 2)), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Resources: " + resources, new Vector2(camX - menuFont.MeasureString("Resources: " + resources).X / 2 + 3, (int)(camY - bounds.Height / 2.5 - menuFont.MeasureString("Resources: " + resources).Y / 2)), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Resources: " + resources, new Vector2(camX - menuFont.MeasureString("Resources: " + resources).X / 2, (int)(camY - bounds.Height / 2.5 - menuFont.MeasureString("Resources: " + resources).Y / 2) - 3), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Resources: " + resources, new Vector2(camX - menuFont.MeasureString("Resources: " + resources).X / 2, (int)(camY - bounds.Height / 2.5 - menuFont.MeasureString("Resources: " + resources).Y / 2) + 3), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Resources: " + resources, new Vector2(camX - menuFont.MeasureString("Resources: " + resources).X / 2, (int)(camY - bounds.Height / 2.5 - menuFont.MeasureString("Resources: " + resources).Y / 2)), new Color(0, 0, menuColor.B, menuColor.A));
                // Raw Resource Count
                spriteBatch.DrawString(menuFont, "Raw Resources: " + rawResources, new Vector2(camX - menuFont.MeasureString("Raw Resources: " + rawResources).X / 2 - 3, (int)(camY + bounds.Height / 2.5 - menuFont.MeasureString("Raw Resources: " + rawResources).Y / 2)), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Raw Resources: " + rawResources, new Vector2(camX - menuFont.MeasureString("Raw Resources: " + rawResources).X / 2 + 3, (int)(camY + bounds.Height / 2.5 - menuFont.MeasureString("Raw Resources: " + rawResources).Y / 2)), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Raw Resources: " + rawResources, new Vector2(camX - menuFont.MeasureString("Raw Resources: " + rawResources).X / 2, (int)(camY + bounds.Height / 2.5 - menuFont.MeasureString("Raw Resources: " + rawResources).Y / 2) - 3), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Raw Resources: " + rawResources, new Vector2(camX - menuFont.MeasureString("Raw Resources: " + rawResources).X / 2, (int)(camY + bounds.Height / 2.5 - menuFont.MeasureString("Raw Resources: " + rawResources).Y / 2) + 3), new Color(0, 0, 0, menuColor.A));
                spriteBatch.DrawString(menuFont, "Raw Resources: " + rawResources, new Vector2(camX - menuFont.MeasureString("Raw Resources: " + rawResources).X / 2, (int)(camY + bounds.Height / 2.5 - menuFont.MeasureString("Raw Resources: " + rawResources).Y / 2)), new Color(menuColor.R, menuColor.G, 0, menuColor.A));
                // Conversion prompt
                if (rawResources > 0)
                {
                    spriteBatch.DrawString(smallMenuFont, "Press B to convert Raw Resources", new Vector2(camX - smallMenuFont.MeasureString("Press B to convert Raw Resources").X / 2 - 2, (int)(camY + bounds.Height / 2.15 - smallMenuFont.MeasureString("Press B to convert Raw Resources").Y / 2)), new Color(0, 0, 0, menuColor.A));
                    spriteBatch.DrawString(smallMenuFont, "Press B to convert Raw Resources", new Vector2(camX - smallMenuFont.MeasureString("Press B to convert Raw Resources").X / 2 + 2, (int)(camY + bounds.Height / 2.15 - smallMenuFont.MeasureString("Press B to convert Raw Resources").Y / 2)), new Color(0, 0, 0, menuColor.A));
                    spriteBatch.DrawString(smallMenuFont, "Press B to convert Raw Resources", new Vector2(camX - smallMenuFont.MeasureString("Press B to convert Raw Resources").X / 2, (int)(camY + bounds.Height / 2.15 - smallMenuFont.MeasureString("Press B to convert Raw Resources").Y / 2) - 2), new Color(0, 0, 0, menuColor.A));
                    spriteBatch.DrawString(smallMenuFont, "Press B to convert Raw Resources", new Vector2(camX - smallMenuFont.MeasureString("Press B to convert Raw Resources").X / 2, (int)(camY + bounds.Height / 2.15 - smallMenuFont.MeasureString("Press B to convert Raw Resources").Y / 2) + 2), new Color(0, 0, 0, menuColor.A));
                    spriteBatch.DrawString(smallMenuFont, "Press B to convert Raw Resources", new Vector2(camX - smallMenuFont.MeasureString("Press B to convert Raw Resources").X / 2, (int)(camY + bounds.Height / 2.15 - smallMenuFont.MeasureString("Press B to convert Raw Resources").Y / 2)), new Color(menuColor.R, 0, 0, menuColor.A));
                }
                //
                foreach (HubButton button in buttons)
                {
                    button.Draw(spriteBatch);
                }
            }
            else
            {
                spriteBatch.DrawString(menuFont, enterPrompt[0], new Vector2(-menuFont.MeasureString(enterPrompt[0]).X / 2, -menuFont.MeasureString(enterPrompt[0]).Y / 2), Color.White);
            }
            if(menuColor.A == 0 && buttons.Count != 0)
                buttons.Clear();
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
