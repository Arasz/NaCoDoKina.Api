namespace NaCoDoKina.Api.DataContracts
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
        /// User location, search area center 
        /// </summary>
        public Location Center { get; }

        /// <summary>
        /// Search radius 
        /// </summary>
        public double Radius { get; }

        public override string ToString() => $"C: {Center}, R: {Radius}";
    }
}