namespace NaCoDoKina.Api.DataContracts.Movies
{
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Movie id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Movie name 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public string PosterUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}";
        }
    }
}