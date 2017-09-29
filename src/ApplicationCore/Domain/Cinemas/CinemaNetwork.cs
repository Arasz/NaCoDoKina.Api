using NaCoDoKina.Api.Entities.Resources;

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
        /// Website to cinema network site 
        /// </summary>
        public ResourceLink Url { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Name)}: {Name}, {nameof(Url)}: {Url}";
        }
    }
}