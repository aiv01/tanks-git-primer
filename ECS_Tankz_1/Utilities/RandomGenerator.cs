using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public static class RandomGenerator
    {
        private static Random rand;

        static RandomGenerator()
        {
            rand = new Random();
        }

        public static int GetRandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static float GetRandomFloat(float min, float max)
        {
            return (float) (min + rand.NextDouble() * (max - min));
        }
    }
}
