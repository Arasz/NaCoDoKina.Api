using NaCoDoKina.Api.DataContracts.Resources;
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
        public long Id { get; set; }

        /// <summary>
        /// Movie description 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Links to movie review services 
        /// </summary>
        public List<ReviewLink> MovieReviews { get; set; }

        /// <summary>
        /// Links to external media resources. <example> Trailer, poster </example> 
        /// </summary>
        public List<MediaLink> MediaResources { get; set; }

        /// <summary>
        /// Movie title 
        /// </summary>
        public string Title { get; set; }

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
        /// Movie age limit 
        /// </summary>
        public string AgeLimit { get; set; }

        /// <summary>
        /// Movie release date 
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Movie director 
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// Movie crew description 
        /// </summary>
        public string CrewDescription { get; set; }

        /// <summary>
        /// Movie rating 
        /// </summary>
        public double Rating { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Description)}: {Description}, {nameof(OriginalTitle)}: {OriginalTitle}, {nameof(Rating)}: {Rating}";
        }
    }
}