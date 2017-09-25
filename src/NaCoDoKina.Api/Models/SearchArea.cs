using System.Globalization;

namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// Cinemas search area 
    /// </summary>
    public class SearchArea
    {
        public SearchArea()
        {
        }

        public SearchArea(Location center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// RegisterUser location, search area center 
        /// </summary>
        public Location Center { get; set; }

        /// <summary>
        /// Search radius 
        /// </summary>
        public double Radius { get; set; }

        public override string ToString()
        {
            return $"{nameof(Center)}: {Center}, {nameof(Radius)}: {Radius.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}