using NaCoDoKina.Api.Entities.Cinemas;

namespace NaCoDoKina.Api.Entities.Movies
{
    /// <summary>
    /// Movie id in cinema network 
    /// </summary>
    public class ExternalId : Entity
    {
        /// <summary>
        /// Movie id in cinema network 
        /// </summary>
        public string MovieExternalId { get; set; }

        /// <summary>
        /// Cinema network in which this id is valid 
        /// </summary>
        public CinemaNetwork CinemaNetwork { get; set; }
    }
}