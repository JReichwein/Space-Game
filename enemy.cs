using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    class enemy : Collidable
    {
        float x;
        float y;
        Vector2 current_pos;
        Vector2 center;
        Texture2D texture;
        float angle;
        int timer = 0;


        List<Missile> missles;
        ContentManager man;
        GameTime time;
        Random rand = new Random();
        float rand_speed;
        float health;

        SpriteFont font;
       public bool dead = false;


        List<Collidable> missles_collidables;

        public enemy(ContentManager man, float x, float y) : base(new vec2(0,0,0,0))
        {
            
            this.man = man;
            angle = 0.0f;
            this.x = x;
            this.y = y;
            this.health = 1000;
            current_pos = new Vector2(x, y);
            texture = man.Load<Texture2D>("player");
            font = man.Load<SpriteFont>("font");
            center = new Vector2(texture.Width / 2, texture.Height / 2);
            missles = new List<Missile>();
            rand_speed = (float)rand.Next(50,200);

            missles_collidables = new List<Collidable>();

            base.setRect(new vec2(x,y,texture.Width, texture.Height));
            base.setRef(this);
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
            missles.Add(new Missile(man, current_pos, angle, center));
            missles_collidables.Add(missles[missles.Count-1]);
        }

        public void stopColision()
        {
            base.exitCollision();
        }

        public void update(Vector2 p1, GameTime time)
        {
            if (base.isColliding)
                if (base.colliding is Missile)
                {
                    Missile x = (Missile)base.colliding;
                    Rectangle rec = new Rectangle((int)current_pos.X, (int)current_pos.Y, texture.Width, texture.Height);
                    if (!x.getRect().Intersects(rec))
                    {
                        base.exitCollision();
                        x.exitCollision();
                    }
                    else
                    { 
                        health--;
                    }
                }

            if (health == 0)
                dead = true;

            this.time = time;
            float distance = Vector2.Distance(p1, current_pos);
            if (distance < 200)
            {
                if (timer > 15)
                {
                    timer = 0;
                    shoot();
                }
                lookAt(p1);
                moveTo(p1);
            }



            foreach (Missile missile in missles)
                missile.Update(time);

            timer++;

            base.setRect(new vec2(x, y, texture.Width, texture.Height));
        }

        public void draw(SpriteBatch pen)
        {
            pen.Draw(texture, current_pos, null, Color.White, angle, center, 1.0f, SpriteEffects.None, 0f);
            pen.DrawString(font, health + "", current_pos, Color.LightPink);
            foreach (Missile missile in missles)
                missile.Draw(pen, time, new Color(rand.Next(0,255), rand.Next(0, 255), rand.Next(0, 255)));
        }
    }
}