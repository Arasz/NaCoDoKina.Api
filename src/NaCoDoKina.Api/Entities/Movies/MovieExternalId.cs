using NaCoDoKina.Api.Entities.Cinemas;

namespace NaCoDoKina.Api.Entities.Movies
{
    /// <summary>
    /// Movie id in cinema system 
    /// </summary>
    public class MovieCinemaId : Entity
    {
        /// <summary>
        /// External id 
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Cinema network in which this id is valid 
        /// </summary>
        public CinemaNetwork CinemaNetwork { get; set; }
    }
}