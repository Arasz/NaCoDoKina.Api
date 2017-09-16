namespace NaCoDoKina.Api.DataContracts
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
        public string Name { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public string PosterUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
}