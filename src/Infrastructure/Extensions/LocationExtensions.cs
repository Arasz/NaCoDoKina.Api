using Infrastructure.Models;

namespace Infrastructure.Extensions
{
    public static class LocationExtensions
    {
        /// <summary>
        /// Returns location as string with lower precision that can be used as cache key 
        /// </summary>
        /// <param name="location"> Location used as cache key </param>
        /// <param name="precision"> Location precision </param>
        /// <returns> Cache key </returns>
        public static string ToCacheKey(this Location location, int precision = 2)
        {
            return location.LowerPrecision(precision).ToString();
        }
    }
}