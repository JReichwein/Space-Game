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
        double astroid_perctange = .25;
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

            int rockCount = 0;

            for (int i = 0; i < len; i++)
            {
                for (int y = 0; y < len; y++)
                {
                    if (world_int[i, y] <= astroid_perctange)
                    {
                        world[i, y] = "R";
                        rockCount++;
                    }
                    else
                    {
                        world[i, y] = "E";
                    }
                }
            }

            Console.WriteLine("Rocks: " + rockCount);
        }
    }

}