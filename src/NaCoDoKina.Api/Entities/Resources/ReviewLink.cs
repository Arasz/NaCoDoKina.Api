namespace NaCoDoKina.Api.Entities.Resources
{
    /// <summary>
    /// Link to website with movie reviews <example> IMDb, Filmweb, etc. </example> 
    /// </summary>
    public class ReviewLink : ResourceLink
    {
        /// <summary>
        /// Link to service logo 
        /// </summary>
        public ResourceLink Logo { get; set; }

        /// <summary>
        /// Title of service with review 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Movie rating 
        /// </summary>
        public double Rating { get; set; }
    }
}