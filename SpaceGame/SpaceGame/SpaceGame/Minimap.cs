using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    class Minimap
    {
        string[,] map;
        Texture2D text;
        public Minimap(ContentManager man)
        {
            map = new string[100,100];
            text = man.Load<Texture2D>("Player");
        }

        public void update(List<Asteroid> astr, List<enemy> ene, Player p1)
        {
            map[50,50] = "p";

            for (int i = 0; i < astr.Count; i++)
            {
                if (Vector2.Distance(astr[i].Position, p1.player_pos) <= 500)
                {
                    map[(int)astr[i].Position.X / 700, (int)astr[i].Position.Y / 700] = "e";
                }
            }

        }

        public void draw(SpriteBatch pen)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int y = 0; y < 100; y++)
                {
                    if (map[i, y] == "e")
                        pen.Draw(text, new Rectangle(i*20, i*20, 50, 50), Color.White);
                }
            }
        }
    }
}
