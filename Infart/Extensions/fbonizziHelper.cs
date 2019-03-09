using System;

namespace fge
{
    public static class fbonizziHelper
    {
        public static Random random = new Random();

        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
