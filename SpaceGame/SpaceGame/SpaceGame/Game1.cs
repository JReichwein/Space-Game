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
        SpriteFont menuFont;

        Player p1;
        Hub hub;
        Camera2D m_camera;
        map mp;
        Texture2D rock;
        ThreadStart update;
        Thread drawing;
        List<Rectangle> draw;
        List<Asteroid> asteroids;
        bool drawCall = false;

        public static int MAX_RENDER_DISTANCE = 500;

        public Game1()
        {
            this.IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            //graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
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
                            //draw.Add(new Rectangle(i * 50, y * 50, 20, 20));
                            asteroids.Add(new Asteroid(this.Content, new Vector2(i * 50, y * 50)));
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
            asteroids = new List<Asteroid>();
            drawing = new Thread(update_);
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
            KeyboardState kB = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            if (pad.Buttons.Back == ButtonState.Pressed || kB.IsKeyDown(Keys.Escape))
                this.Exit();

            p1.update(gameTime, Content, hub.isInHub());
            hub.Update(gameTime, pad, p1, m_camera, Content, menuFont);

            // Check if player touches an asteroid in order to start mining
            foreach (Asteroid asteroid in asteroids) {
                if (p1.getRectangle().Intersects(asteroid.Rectangle))
                {
                    Console.WriteLine("Mine Asteroid");
                    //asteroid.startMining();
                }
            }


            m_camera.Location = p1.player_pos;
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

            if (hub.isOnCamera(m_camera))
            {
                hub.Draw(spriteBatch);
            }

            p1.draw(spriteBatch, gameTime);

            //for (int i = 0; i < draw.Count; i+=0)
            for (int i = 0; i < asteroids.Count; i += 0)
            {
                //spriteBatch.Draw(rock, draw[i], Color.LightPink);
                asteroids[i].Draw(gameTime, spriteBatch);
                //draw.RemoveAt(i);
                asteroids.RemoveAt(i);
            }
            drawCall = true;

            if (hub.isOnCamera(m_camera))
            {
                hub.DrawRadius(spriteBatch);
                if (hub.isWithinRadius(p1.getRectangle()))
                    hub.DrawMenu(spriteBatch, menuFont, m_camera);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
