namespace NaCoDoKina.Api.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Cinema network entity 
    /// </summary>
    public class CinemaNetwork : Entity
    {
        /// <summary>
        /// Network name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url to cinema network site 
        /// </summary>
        public ServiceUrl Url { get; set; }
    }
}