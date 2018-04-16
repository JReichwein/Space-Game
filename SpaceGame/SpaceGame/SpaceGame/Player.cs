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
    class Camera2D
    {
        public float Zoom { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom) *
                Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
        }


        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
        }
    };

    class Player
    {
  

        private Texture2D texture;
        public Vector2 player_pos;
        private double x = 300;
        private double y = 300;
        private double ang = 0;
        private Vector2 origin;
        private double vel = 1.2;
        private double oldang = 0;

        public Player(ContentManager man)
        {
            texture = man.Load<Texture2D>("Player");
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            player_pos = new Vector2((int)x, (int)y);
        }

        public void update()
        {
            controller();

            player_pos.X = (int)x;
            player_pos.Y = (int)y;
        }

        Vector2 angleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        public void controller()
        {
            GamePadState pad = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
            ang = (float)MathHelper.ToDegrees((float)ang);



            ang = MathHelper.ToDegrees((float)Math.Atan2(pad.ThumbSticks.Right.X, pad.ThumbSticks.Right.Y));
            if (ang < 0)
                ang += 360;
            if (ang == 0)
                ang = 0;

            if (pad.ThumbSticks.Right.X != 0 || pad.ThumbSticks.Right.Y != 0)
            {
                Vector2 new_pos = angleToVector(MathHelper.ToRadians((float)ang));

                x += new_pos.X * .05;
                y += new_pos.Y * .05;

                oldang = ang;
                ang = (float)MathHelper.ToRadians((float)ang);
            }
            else
            {
                ang = (float)MathHelper.ToRadians((float)oldang);
            }


        }

        public void draw(SpriteBatch pen)
        {
            pen.Draw(texture, player_pos, null, Color.White, (float)ang, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
