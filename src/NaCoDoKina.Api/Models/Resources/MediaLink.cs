namespace NaCoDoKina.Api.Models.Resources
{
    /// <summary>
    /// Link to media resource 
    /// </summary>
    public class MediaLink
    {
        /// <summary>
        /// Media url 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Type of resource 
        /// </summary>
        public MediaType MediaType { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(MediaType)}: {MediaType}";
        }
    }
}