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
        private Rectangle buttonRect, progressRect, progressBarRect;
        private Texture2D buttonText, progressText, progressBarText;
        private Color buttonColor, textColor;
        private String text, type;
        private Vector2 textSize;
        private MouseState oldMouse;
        private SpriteFont buttonFont;
        private int requiredResources;
        private double progressLength /* in percent, must be < 1 */, statMax;

        public HubButton(Vector2 position, String text, ContentManager manager, Player p1)
        {
            type = text.Substring(text.IndexOf(" ") + 1);
            buttonText = manager.Load<Texture2D>("Button");
            progressText = manager.Load<Texture2D>("Progress");
            progressBarText = manager.Load<Texture2D>("ProgressBar");
            buttonFont = manager.Load<SpriteFont>("ButtonFont");
            statMax = chooseStatMax(p1);
            progressLength = chooseBarLength(p1);
            if (progressLength > 1)
                progressLength = 1;
            requiredResources = chooseRequiredResources();
            if (requiredResources != 0)
                this.text = text + " (" + requiredResources + ")";
            else
                this.text = text + " (UNAVAILABLE)";
            textSize = new Vector2((int)(buttonFont.MeasureString(this.text).X), (int)(buttonFont.MeasureString(this.text).Y));
            buttonRect = new Rectangle((int)position.X - (int)textSize.X / 2 - 18, (int)position.Y - (int)textSize.Y / 2 - 7, (int)textSize.X + 36, (int)textSize.Y + 14);
            progressBarRect = new Rectangle((int)position.X - 30, (int)position.Y + buttonRect.Height / 2, 60, 10);
            progressRect = new Rectangle(progressBarRect.X + 1, progressBarRect.Y, (int)(progressBarRect.Width * progressLength), progressBarRect.Height);
            buttonColor = new Color(1, 1, 1, 0);
            textColor = Color.Black;
            oldMouse = Mouse.GetState();
        }

        public void Update(GameTime gameTime, Camera2D camera, Color menuColor)
        {
            MouseState mouse = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
            mousePosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.TransformMatrix()));

            if (isTouchingButton(mousePosition))
            {
                buttonColor = Color.Red;
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

        public void updateBar(Player p1)
        {
            progressLength = chooseBarLength(p1);
            if (progressLength > 1)
                progressLength = 1;
            progressRect = new Rectangle(progressBarRect.X + 1, progressBarRect.Y, (int)(progressBarRect.Width * progressLength), progressBarRect.Height);

            requiredResources = chooseRequiredResources();
            if (requiredResources != 0)
                this.text = text.Substring(0, text.LastIndexOf(" ")) + " (" + requiredResources + ")";
            else
                this.text = text + " (UNAVAILABLE)";
            textSize = new Vector2((int)(buttonFont.MeasureString(this.text).X), (int)(buttonFont.MeasureString(this.text).Y));
            buttonRect = new Rectangle(buttonRect.X, buttonRect.Y, (int)textSize.X + 36, (int)textSize.Y + 14);
        }

        public bool isTouchingButton(Vector2 position)
        {
            // add 300 to the position of each component for proper detections.
            return new Rectangle((int)position.X, (int)position.Y, 1, 1).Intersects(buttonRect);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            spriteBatch.Draw(buttonText, buttonRect, buttonColor);
            spriteBatch.DrawString(buttonFont, text, new Vector2(buttonRect.X + (buttonRect.Width - buttonFont.MeasureString(text).X) / 2, buttonRect.Y + (buttonRect.Height - buttonFont.MeasureString(text).Y) / 2), textColor);
            spriteBatch.Draw(progressBarText, progressBarRect, new Color(buttonColor.R, buttonColor.R, buttonColor.R, buttonColor.A));
            spriteBatch.Draw(progressText, progressRect, new Color(buttonColor.R, buttonColor.R, buttonColor.R, buttonColor.A));
        }

        public bool isButton(String name)
        {
            return name.Equals(text);
        }

        public string getType()
        {
            return type;
        }

        public int getRequiredResources()
        {
            return requiredResources;
        }

        public double getStatMax()
        {
            return statMax;
        }

        private double chooseStatMax(Player p1)
        {
            double max = 0;
            if (type.Equals("SPEED"))
            {
                max = 2.4;
            }
            else if (type.Equals("ARMOR"))
            {
                max = 10;
            }
            else if (type.Equals("DAMAGE"))
            {
                max = 20;
            }
            else if (type.Equals("RATE OF FIRE"))
            {
                max = 250;
            }
            else if (type.Equals("STORAGE"))
            {
                max = 1000;
            }
            return max;
        }

        private double chooseBarLength(Player p1)
        {
            double length = 0.0;
            if (type.Equals("SPEED"))
            {
                length = p1.getTopSpeed() / statMax;
            }
            else if (type.Equals("ARMOR"))
            {
                length = p1.getArmor() / statMax;
            }
            else if (type.Equals("DAMAGE"))
            {
                length = p1.getDamage() / statMax;
            }
            else if (type.Equals("RATE OF FIRE"))
            {
                length = statMax / p1.getRateOfFire();
            }
            else if (type.Equals("STORAGE"))
            {
                length = p1.getStorage() / statMax;
            }
            return length;
        }

        private int chooseRequiredResources()
        {
            if (progressLength == 1)
                return 0;
            if (type.Equals("SPEED"))
            {
                return (int)(50 + 500 * progressLength);
            }
            else if (type.Equals("ARMOR"))
            {
                return (int)(50 + 750 * progressLength);
            }
            else if (type.Equals("DAMAGE"))
            {
                return (int)(50 + 375 * progressLength);
            }
            else if (type.Equals("RATE OF FIRE"))
            {
                return (int)(50 + 400 * progressLength);
            }
            else if (type.Equals("STORAGE"))
            {
                return (int)(50 + 425 * progressLength);
            }
            return -1;
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
        private MouseState oldMouse;

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
            oldMouse = Mouse.GetState();
        }

        public void Update(GameTime gameTime, GamePadState pad, Player player, Camera2D camera, ContentManager manager, Player p1)
        {

            if (isWithinRadius(player.getRectangle()) && pad.Buttons.X == ButtonState.Pressed && !menuOpened)
            {
                menuOpened = true;
                Rectangle bounds = camera.getBounds();
                Vector2 center = new Vector2(camera.Location.X, camera.Location.Y);
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y - camera.getBounds().Height / 3.5)), "UPGRADE SPEED", manager, p1));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y - camera.getBounds().Height / 7)), "UPGRADE ARMOR", manager, p1));
                buttons.Add(new HubButton(new Vector2(center.X, center.Y), "UPGRADE DAMAGE", manager, p1));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y + camera.getBounds().Height / 7)), "UPGRADE RATE OF FIRE", manager, p1));
                buttons.Add(new HubButton(new Vector2(center.X, (int)(center.Y + camera.getBounds().Height / 3.5)), "UPGRADE STORAGE", manager, p1));
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

            if (menuColor.A != 0)
            {
                MouseState mouse = Mouse.GetState();
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                mousePosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.TransformMatrix()));

                if (pad.Buttons.B == ButtonState.Pressed && p1.getRawResources() > 0)
                {
                    p1.setResources(p1.getRawResources() + p1.getResources());
                    p1.setRawResources(0);
                }

                foreach (HubButton button in buttons)
                {
                    button.Update(gameTime, camera, menuColor);

                    if (menuOpened)
                    {
                        if (button.isTouchingButton(mousePosition) && mouse.LeftButton == ButtonState.Pressed && !(oldMouse.LeftButton == ButtonState.Pressed))
                        {
                            if (button.getRequiredResources() <= p1.getResources())
                            {
                                string type = button.getType();
                                if (type.Equals("SPEED") && p1.getTopSpeed() < button.getStatMax())
                                {
                                    p1.setTopSpeed(p1.getTopSpeed() + 0.2);
                                    p1.setResources(p1.getResources() - button.getRequiredResources());
                                }
                                else if (type.Equals("ARMOR") && p1.getArmor() < button.getStatMax())
                                {
                                    p1.setArmor(p1.getArmor() + 1.0);
                                    p1.setResources(p1.getResources() - button.getRequiredResources());
                                }
                                else if (type.Equals("DAMAGE") && p1.getDamage() < button.getStatMax())
                                {
                                    p1.setDamage(p1.getDamage() + 1.0);
                                    p1.setResources(p1.getResources() - button.getRequiredResources());
                                }
                                else if (type.Equals("RATE OF FIRE") && p1.getRateOfFire() > button.getStatMax())
                                {
                                    p1.setRateOfFire(p1.getRateOfFire() - 50);
                                    p1.setResources(p1.getResources() - button.getRequiredResources());
                                }
                                else if (type.Equals("STORAGE") && p1.getStorage() < button.getStatMax())
                                {
                                    p1.setStorage(p1.getStorage() + 50);
                                    p1.setResources(p1.getResources() - button.getRequiredResources());
                                }
                                else
                                {
                                    Console.Beep(1500, 200);
                                    continue;
                                }
                                button.updateBar(p1);
                                Console.Beep(3000, 100);
                                Console.Beep(3000, 100);
                            }
                            else
                            {
                                Console.Beep(1500, 200);
                            }
                        }
                    }
                }

                oldMouse = mouse;
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
            if (menuColor.A != 0)
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
            if (menuColor.A == 0 && buttons.Count != 0)
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