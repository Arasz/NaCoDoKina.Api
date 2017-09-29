namespace NaCoDoKina.Api.Entities.Resources
{
    /// <summary>
    /// Types of resources that link can refer to 
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Movie poster 
        /// </summary>
        Poster,

        /// <summary>
        /// Movie trailer 
        /// </summary>
        Trailer,

        /// <summary>
        /// Movie teaser (short, early stage trailer) 
        /// </summary>
        Teaser,

        /// <summary>
        /// Any other video 
        /// </summary>
        Video,

        /// <summary>
        /// Any picture 
        /// </summary>
        Picture,
    }
}