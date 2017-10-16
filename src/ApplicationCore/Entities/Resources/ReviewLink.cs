namespace ApplicationCore.Entities.Resources
{
    /// <summary>
    /// Link to website with movie reviews <example> IMDb, Filmweb, etc. </example> 
    /// </summary>
    public class ReviewLink : Entity
    {
        /// <summary>
        /// Url to review 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Link to service logo 
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Title of service with review 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Movie rating 
        /// </summary>
        public double Rating { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(LogoUrl)}: {LogoUrl}, {nameof(Name)}: {Name}, {nameof(Rating)}: {Rating}";
        }
    }
}