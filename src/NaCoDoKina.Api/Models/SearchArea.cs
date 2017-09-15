namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// Cinemas search area 
    /// </summary>
    public class SearchArea
    {
        public SearchArea(Location center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// User center, center 
        /// </summary>
        public Location Center { get; }

        /// <summary>
        /// Search radius 
        /// </summary>
        public double Radius { get; }

        public override string ToString() => $"(Location: {Center}, Radius: {Radius})";
    }
}