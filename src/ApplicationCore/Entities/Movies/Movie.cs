﻿using System.Collections.Generic;

namespace ApplicationCore.Entities.Movies
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie : Entity
    {
        /// <summary>
        /// Movie name (title) 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Movie representation in cinema network 
        /// </summary>
        public List<ExternalMovie> ExternalMovies { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public string PosterUrl { get; set; }

        /// <summary>
        /// Detailed information about movie 
        /// </summary>
        public MovieDetails Details { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Title)}: {Title}";
        }
    }
}