using System;

namespace Infart.Extensions
{
    public static class FbonizziHelper
    {
        public static Random Random = new Random();

        public static float RandomBetween(float min, float max)
        {
            return min + (float)Random.NextDouble() * (max - min);
        }
    }
}