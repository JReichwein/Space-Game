using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGame
{
    public class map
    {
        public string[,] world;
        double[,] world_int;
        double space_perctange = .30;
        double astroid_perctange = .40;
        public int len = 0;

        public map(int len)
        {
            world = new string[len, len];
            world_int = new double[len, len];
            this.len = len;

        }

        void make_map()
        {
            Random rand = new Random();

            for (int i = 0; i < len; i++)
            {
                for (int y = 0; y < len; y++)
                {
                    world_int[i, y] = rand.NextDouble();
                }
            }
        }

        double get_average(int x, int y)
        {
            double average = 0;
            if (x > 0)
                average += world_int[x - 1, y];

            if (x < len - 1)
                average += world_int[x + 1, y];

            if (y > 0)
                average += world_int[x, y - 1];

            if (y < len - 1)
                average += world_int[x, y + 1];

            if (average > 0)
                return average / 4;
            else
                return world_int[x, y];
        }

        void beautify()
        {
            for (int i = 0; i < len; i++)
            {
                for (int y = 0; y < len; y++)
                {
                    world_int[i, y] = get_average(i, y);
                }
            }
        }

        public void generate()
        {

            make_map();
            beautify();

            for (int i = 0; i < len; i++)
            {
                for (int y = 0; y < len; y++)
                {
                    if (world_int[i, y] < .75)
                    {
                        world[i, y] = "E";
                    }
                    else if (world_int[i, y] < .9)
                    {
                        world[i, y] = "R";
                    }
                }
            }


        }
    }

}