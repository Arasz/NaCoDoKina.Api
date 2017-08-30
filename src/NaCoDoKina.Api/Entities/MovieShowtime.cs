using System;

namespace NaCoDoKina.Api.Entities
{
    public class MovieShowtime : Entity
    {
        public DateTime DateTime { get; set; }

        public Cinema Cinema { get; set; }

        public Movie Movie { get; set; }

        /// <summary>
        /// If false, movie show time is before current date time. 
        /// </summary>
        public bool CanBeWatched { get; set; }
    }
}