using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    public class map
    {
        public List<Vector2> mp;
        static Random rand = new Random();

        public map()
        {
            mp = new List<Vector2>();
        }

        public void make_map(int radius, Hub hb)
        {
            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    pos.X *= 20;
                    pos.Y *= 20;
                    if (rand.NextDouble() < .01 && !hb.isWithinRadius(new Rectangle((int)pos.X, (int)pos.Y, 20, 20)))
                    {
                        mp.Add(pos);
                    }  
                }
            }
        }
           
    }

}