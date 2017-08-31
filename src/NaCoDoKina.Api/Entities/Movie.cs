using System.Collections.Generic;

namespace NaCoDoKina.Api.Entities
{
    public class Movie : Entity
    {
        public string Title { get; set; }

        public MovieDetails Details { get; set; }

        public IEnumerable<MovieShowtime> MovieShowtimes { get; set; }
    }
}