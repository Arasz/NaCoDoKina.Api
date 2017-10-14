namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Area in which we are are searching for cinemas 
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
        /// User location as geographic coordinates, search area center 
        /// </summary>
        public Location Center { get; set; }

        /// <summary>
        /// Search radius in meters 
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// City in which search is conducted. 
        /// </summary>
        public string City { get; set; }

        public override string ToString()
        {
            return $"{nameof(Center)}: {Center}, {nameof(Radius)}: {Radius}";
        }
    }
}