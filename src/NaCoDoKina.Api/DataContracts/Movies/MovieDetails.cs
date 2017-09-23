using System;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataContracts.Movies
{
    /// <summary>
    /// Detailed information about show 
    /// </summary>
    public class MovieDetails
    {
        /// <summary>
        /// Movie id 
        /// </summary>
        public long MovieId { get; set; }

        /// <summary>
        /// Sites with movie descriptions 
        /// </summary>
        public List<ServiceUrl> DescriptionSites { get; set; }

        /// <summary>
        /// Movie description 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Original movie title 
        /// </summary>
        public string OriginalTitle { get; set; }

        /// <summary>
        /// Movie genre 
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Movie length 
        /// </summary>
        public TimeSpan Length { get; set; }

        /// <summary>
        /// Movie original language 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Movie age limit 
        /// </summary>
        public string AgeLimit { get; set; }

        /// <summary>
        /// Movie release date 
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Movie rating for current user 
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Movie director 
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Movie crew description 
        /// </summary>
        public string CrewDescription { get; set; }

        public override string ToString()
        {
            return $"{nameof(MovieId)}: {MovieId}, {nameof(Description)}: {Description}, {nameof(OriginalTitle)}: {OriginalTitle}, {nameof(Rating)}: {Rating}";
        }
    }
}