﻿using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Exceptions
{
    public class MovieNotAddedException : NaCoDoKinaApiException
    {
        public MovieNotAddedException(Movie movie)
            : base($"Movie ${movie} not added because of internal error")
        {
        }
    }

    public class MovieDetailsNotAddedException : NaCoDoKinaApiException
    {
        public MovieDetailsNotAddedException(MovieDetails movieDetails)
            : base($"Movie details ${movieDetails} not added because of internal error")
        {
        }
    }
}