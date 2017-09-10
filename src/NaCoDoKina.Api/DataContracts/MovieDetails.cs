using System.Collections.Generic;

namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Detailed informations about show 
    /// </summary>
    public class MovieDetails
    {
        public string MovieId { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        public string OriginalTitle { get; set; }

        public string Genre { get; set; }

        public string Length { get; set; }

        public string Language { get; set; }

        public string ReleaseDate { get; set; }

        public string AgeLimit { get; set; }

        public string Director { get; set; }

        public string Crew { get; set; }

        public string Production { get; set; }

        public List<MovieShowtime> MovieShowtimes { get; set; }
    }
}