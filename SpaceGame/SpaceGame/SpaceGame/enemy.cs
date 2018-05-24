using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    class enemy 
    {
        float x;
        float y;
        Vector2 current_pos;
        Vector2 center;
        Texture2D texture;
        float angle;
        int timer = 0;


        Missile last;
        ContentManager man;
        GameTime time;
        Random rand = new Random();
        float rand_speed;
        float health;

        SpriteFont font;
        public bool dead = false;


        public enemy(ContentManager man, float x, float y) 
        {
            
            this.man = man;
            angle = 0.0f;
            this.x = x;
            this.y = y;
            this.health = 10;
            current_pos = new Vector2(x, y);
            texture = man.Load<Texture2D>("Enemy");
            font = man.Load<SpriteFont>("SmallMenuFont");
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            last = null;
            rand_speed = (float)rand.Next(50,200);


        }

        float lookAt(Vector2 p1)
        {
            return (float)Math.Atan2(p1.X, -p1.Y) ;
            
        }

        Vector2 angleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        void moveTo(Vector2 p1)
        {
            var delta = (float)time.ElapsedGameTime.TotalSeconds;

            Vector2 dir = p1 - current_pos;
            dir.Normalize();
            current_pos += dir * delta * rand_speed;
            angle = lookAt(dir);
            
        }

        void shoot()
        {
            last = new Missile(man, current_pos, angle, center, 4);
            last.tag = "en";
        }

        public Missile update(Vector2 p1, GameTime time, List<Asteroid> astroids, List<Missile> bullets)
        {

            for (int i = 0; i < bullets.Count; i++)
                if (bullets[i].Rectangle.Intersects(new Rectangle((int)current_pos.X, (int)current_pos.Y, texture.Width, texture.Height)))
                {
                    if (bullets[i].tag == "player")
                    {
                        health--;
                        bullets[i].collided = true;
                    }
                }

            if (health == 0)
                dead = true;
            last = null;

            this.time = time;
            float distance = Vector2.Distance(p1, current_pos);
            if (distance < 300 && distance > 100)
            {
                if (timer > 60)
                {
                    timer = 0;
                    shoot();
                }
                lookAt(p1);
                moveTo(p1);
            }
            else if (distance < 300 && distance < 100)
            {
                if (timer > 60)
                {
                    timer = 0;
                    shoot();
                }
            }


            timer++;
            return last;
        }

        public void draw(SpriteBatch pen)
        {
            pen.Draw(texture, current_pos, null, Color.White, angle, center, 1.0f, SpriteEffects.None, 0f);
            pen.DrawString(font, health + "", current_pos, Color.LightPink);
        }
    }
}