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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont menuFont, titleFont;

        Player p1;
        Hub hub;
        Camera2D m_camera;
        map mp;
        Texture2D rock;
        ThreadStart update;
        Thread drawing;
        List<Rectangle> draw;
        MouseState oldMouse = Mouse.GetState();
        String[] mainMenuText;
        Rectangle[] mainMenuButtons;
        Color[] mainMenuColors;
        Rectangle[] menuObjects;
        Texture2D[] menuTextures;

        bool drawCall = false;

        enum GameState { MainMenu, Controls, Credits, Playing };
        GameState state = GameState.MainMenu;

        public static int MAX_RENDER_DISTANCE = 500;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        void drawRocks()
        {
            for (int i = 0; i < mp.len; i++)
            {
                for (int y = 0; y < mp.len; y++)
                {
                    if (mp.world[i, y] == "R")
                    {
                        Vector2 rect = new Vector2(i * 50, y * 50);
                        if (Vector2.Distance(p1.player_pos, rect) <= MAX_RENDER_DISTANCE)
                        {
                            draw.Add(new Rectangle(i * 50, y * 50, 20, 20));
                        }
                    }

                }
            }
        }


        public void update_()
        {
            while (true)
            {
                if (drawCall == true)
                {
                    drawRocks();
                    drawCall = false;
                }
            }
        }

        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            drawing.Abort();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            m_camera = new Camera2D(GraphicsDevice.Viewport);
            m_camera.Zoom = 1;
            update = new ThreadStart(update_);
            draw = new List<Rectangle>();
            drawing = new Thread(update_);
            mainMenuText = new String[4] { "Play", "Controls", "Credits", "Exit" };
            mainMenuColors = new Color[4] { Color.White, Color.White, Color.White, Color.White };
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p1 = new Player(Content);
            hub = new Hub(Content);
            mp = new map(100);
            mp.generate();
            drawing.Start();
            rock = Content.Load<Texture2D>("Asteroid1");
            menuFont = Content.Load<SpriteFont>("MenuFont");
            titleFont = Content.Load<SpriteFont>("TitleFont");
            mainMenuButtons = new Rectangle[4] {
                new Rectangle ((int)-menuFont.MeasureString(mainMenuText[0]).X / 2, (int)-menuFont.MeasureString(mainMenuText[0]).Y * 2, (int)menuFont.MeasureString(mainMenuText[0]).X, (int)menuFont.MeasureString(mainMenuText[0]).Y),
                new Rectangle((int)-menuFont.MeasureString(mainMenuText[1]).X / 2, (int)(-menuFont.MeasureString(mainMenuText[1]).Y * 1), (int)menuFont.MeasureString(mainMenuText[1]).X, (int)menuFont.MeasureString(mainMenuText[1]).Y),
                new Rectangle((int)-menuFont.MeasureString(mainMenuText[2]).X / 2, 0, (int)menuFont.MeasureString(mainMenuText[2]).X, (int)menuFont.MeasureString(mainMenuText[2]).Y),
                new Rectangle((int)-menuFont.MeasureString(mainMenuText[3]).X / 2, (int)menuFont.MeasureString(mainMenuText[3]).Y, (int)menuFont.MeasureString(mainMenuText[3]).X, (int)menuFont.MeasureString(mainMenuText[3]).Y)
            };

            menuObjects = new Rectangle[] {
                new Rectangle(-250, -22, 36, 44),
                new Rectangle(175, -22, 36, 44),
                new Rectangle(250, 125, rock.Width * 2, rock.Height * 2),
                new Rectangle(-325, -200, rock.Width * 2, rock.Height * 2)
            };

            menuTextures = new Texture2D[] {
                Content.Load<Texture2D>("Player"), // Player's Ship
                Content.Load<Texture2D>("Enemy"), // Enemy's Ship
                rock,
                rock
            };
            //menuTextures[0] = Content.Load<Texture2D>(""); // Background

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (state == GameState.Playing)
            {
                KeyboardState kB = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);

                if (pad.Buttons.Back == ButtonState.Pressed || kB.IsKeyDown(Keys.Escape))
                    this.Exit();

                p1.update(gameTime, Content, hub.isInHub());
                hub.Update(gameTime, pad, p1, m_camera, Content, p1);
                // TODO: Add your update logic here

                /*
                if (gameTime.TotalGameTime.TotalSeconds == 1)
                {
                    Console.WriteLine(ticks);
                } else
                    ticks++;
                    */

                m_camera.Location = p1.player_pos;
            }
            else if (state == GameState.MainMenu)
            {
                MouseState mouse = Mouse.GetState();
                Vector2 mousePosition = new Vector2(mouse.X, mouse.Y);
                mousePosition = Vector2.Transform(mousePosition, Matrix.Invert(m_camera.TransformMatrix()));

                for(int i = 0; i < mainMenuButtons.Length; i++)
                {
                    if(mainMenuButtons[i].Intersects(new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1)))
                    {
                        mainMenuColors[i] = Color.Red;
                        if(mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed)
                        {
                            if (mainMenuText[i].Equals("Play"))
                            {
                                state = GameState.Playing;
                            }
                            else if (mainMenuText[i].Equals("Controls"))
                            {
                                //state = GameState.Controls; 
                            }
                            else if (mainMenuText[i].Equals("Credits"))
                            {
                                //state = GameState.Credits;
                            }
                            else if (mainMenuText[i].Equals("Exit"))
                            {
                                this.Exit();
                            }
                                
                        }
                    }
                    else if (mainMenuColors[i] != Color.White)
                        mainMenuColors[i] = Color.White;
                }
            }
            else if (state == GameState.Controls)
            {

            }
            else if (state == GameState.Credits)
            {

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, m_camera.TransformMatrix());

            if (state == GameState.Playing)
            {
                if (hub.isOnCamera(m_camera))
                {
                    hub.Draw(spriteBatch);
                }

                p1.draw(spriteBatch, gameTime);

                if (!drawCall)
                {
                    for (int i = 0; i < draw.Count; i++)
                    {

                        spriteBatch.Draw(rock, draw[i], Color.LightPink);
                        draw.RemoveAt(i);
                        i--;
                    }
                }
                drawCall = true;

                if (hub.isOnCamera(m_camera))
                {
                    hub.DrawRadius(spriteBatch);
                    if (hub.isWithinRadius(p1.getRectangle()))
                        hub.DrawMenu(spriteBatch, m_camera, p1.getResources(), p1.getRawResources());
                }
            }
            else if (state == GameState.MainMenu)
            {
                String title = "Space Game";
                spriteBatch.DrawString(titleFont, title, new Vector2(-titleFont.MeasureString(title).X / 2, -titleFont.MeasureString(title).Y * 3), Color.Green);

                for (int i = 0; i < mainMenuButtons.Length; i++)
                {
                    spriteBatch.DrawString(menuFont, mainMenuText[i], new Vector2(mainMenuButtons[i].X, mainMenuButtons[i].Y), mainMenuColors[i]);
                }
                
                for (int i = 0; i < menuObjects.Length; i++)
                {
                   spriteBatch.Draw(menuTextures[i], menuObjects[i], Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
