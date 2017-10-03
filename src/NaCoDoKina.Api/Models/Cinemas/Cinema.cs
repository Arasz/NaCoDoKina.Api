using NaCoDoKina.Api.Models.Travel;

namespace NaCoDoKina.Api.Models.Cinemas
{
    /// <summary>
    /// Information about cinema 
    /// </summary>
    public class Cinema
    {
        /// <summary>
        /// Cinema id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Cinema name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cinema address 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Cinema site url 
        /// </summary>
        public string CinemaUrl { get; set; }

        /// <summary>
        /// Network name to which cinema belongs 
        /// </summary>
        public string NetworkName { get; set; }

        /// <summary>
        /// Cinema location 
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Information about travel from user location to cinema 
        /// </summary>
        public TravelInformation CinemaTravelInformation { get; set; }
    }
}