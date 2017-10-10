using ApplicationCore.Entities.Cinemas;

namespace ApplicationCore.Entities.Movies
{
    /// <summary>
    /// Movie in cinema network 
    /// </summary>
    public class ExternalMovie : Entity
    {
        /// <summary>
        /// Movie id in cinema network 
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Link to movie on cinema network site 
        /// </summary>
        public string MovieUrl { get; set; }

        /// <summary>
        /// Cinema network in which this id is valid 
        /// </summary>
        public CinemaNetwork CinemaNetwork { get; set; }
    }
}