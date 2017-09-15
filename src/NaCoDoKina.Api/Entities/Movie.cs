﻿namespace NaCoDoKina.Api.Entities
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
        public string Name { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public string PosterUrl { get; set; }

        /// <summary>
        /// Detailed information about movie 
        /// </summary>
        public MovieDetails Details { get; set; }
    }
}