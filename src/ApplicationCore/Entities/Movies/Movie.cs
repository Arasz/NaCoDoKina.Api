using System.Collections.Generic;

namespace ApplicationCore.Entities.Movies
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie : Entity
    {
        public static IEqualityComparer<Movie> TitleComparer { get; } = new TitleEqualityComparer();

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

        private sealed class TitleEqualityComparer : IEqualityComparer<Movie>
        {
            public bool Equals(Movie x, Movie y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.Title, y.Title);
            }

            public int GetHashCode(Movie obj)
            {
                return (obj.Title != null ? obj.Title.GetHashCode() : 0);
            }
        }
    }
}