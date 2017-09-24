using AutoMapper;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class MovieShowtimeService : IMovieShowtimeService
    {
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly ILogger<IMovieShowtimeService> _logger;
        private readonly IMapper _mapper;

        public MovieShowtimeService(IMovieShowtimeRepository movieShowtimeRepository, ILogger<IMovieShowtimeService> logger, IMapper mapper)
        {
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<MovieShowtime>> GetMovieShowtimesAsync(long movieId, DateTime laterThan)
        {
            var showtimes = await _movieShowtimeRepository.GetMovieShowtimesAsync(movieId, laterThan);

            if (showtimes is null || !showtimes.Any())
                throw new MovieShowtimeNotFoundException(movieId);

            return _mapper.MapMany<Entities.MovieShowtime, MovieShowtime>(showtimes);
        }

        public async Task<IEnumerable<MovieShowtime>> GetMovieShowtimesForCinemaAsync(long movieId, long cinemaId, DateTime laterThan)
        {
            var showtimes = await _movieShowtimeRepository.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan);

            if (showtimes is null || !showtimes.Any())
                throw new MovieShowtimeNotFoundException(movieId);

            return _mapper.MapMany<Entities.MovieShowtime, MovieShowtime>(showtimes);
        }
    }
}