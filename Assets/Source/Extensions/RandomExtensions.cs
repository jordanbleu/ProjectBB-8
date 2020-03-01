using System;

namespace Assets.Source.Extensions
{
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a float between <paramref name="min"/> and <paramref name="max"/>
        /// </summary>
        /// <param name="random">The specified random</param>
        /// <param name="min">the min value</param>
        /// <param name="max">the max value</param>
        /// <returns>A random number between min and max</returns>
        public static float NextFloatInRange(this Random random, float min, float max) 
        {
            return (float)(random.NextDouble() * (max - min) + min);
        }
    }
}
