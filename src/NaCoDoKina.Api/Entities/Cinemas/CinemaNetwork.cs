namespace NaCoDoKina.Api.Entities.Cinemas
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
        /// Cinema network site 
        /// </summary>
        public string CinemaNetworkUrl { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}";
        }
    }
}