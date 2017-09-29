namespace NaCoDoKina.Api.Models.Resources
{
    /// <summary>
    /// Link to media resource 
    /// </summary>
    public class MediaLink : ResourceLink
    {
        /// <summary>
        /// Type of resource 
        /// </summary>
        public MediaType MediaType { get; set; }
    }
}