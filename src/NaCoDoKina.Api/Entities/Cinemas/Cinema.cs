namespace NaCoDoKina.Api.Entities.Cinemas
{
    /// <inheritdoc/>
    /// <summary>
    /// Cinema entity 
    /// </summary>
    public class Cinema : Entity
    {
        /// <summary>
        /// Id from external system <example> Cinema web api or site </example> 
        /// </summary>
        public string ExternalId { get; set; }

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
        /// Cinema location 
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// The network to which the movie belongs 
        /// </summary>
        public CinemaNetwork CinemaNetwork { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}, {nameof(Address)}: {Address}, {nameof(Location)}: {Location}, {nameof(CinemaNetwork)}: {CinemaNetwork}";
        }
    }
}