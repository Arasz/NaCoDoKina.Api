namespace NaCoDoKina.Api.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Cinema entity 
    /// </summary>
    public class Cinema : Entity
    {
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
        public ServiceUrl Website { get; set; }

        /// <summary>
        /// Cinema location 
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// The network to which the movie belongs 
        /// </summary>
        public CinemaNetwork CinemaNetwork { get; set; }
    }
}