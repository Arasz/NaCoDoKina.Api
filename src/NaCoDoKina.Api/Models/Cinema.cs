namespace NaCoDoKina.Api.Models
{
    public class Cinema
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string NetworkName { get; set; }

        public Location Location { get; set; }

        /// <summary>
        /// Information about travel from user location to cinema 
        /// </summary>
        public TravelInformation CinemaTravelInformation { get; set; }
    }
}