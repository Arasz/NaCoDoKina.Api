using ApplicationCore.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Models.Movies;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
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
            var showtimes = await _movieShowtimeRepository.GetShowtimesForMovieAsync(movieId, laterThan);

            if (showtimes is null || !showtimes.Any())
                throw new MovieShowtimeNotFoundException(movieId);

            return _mapper.MapMany<ApplicationCore.Entities.Movies.MovieShowtime, MovieShowtime>(showtimes);
        }

        public async Task<IEnumerable<MovieShowtime>> GetMovieShowtimesForCinemaAsync(long movieId, long cinemaId, DateTime laterThan)
        {
            var showtimes = await _movieShowtimeRepository.GetShowtimesForCinemaAndMovieAsync(movieId, cinemaId, laterThan);

            if (showtimes is null || !showtimes.Any())
                throw new MovieShowtimeNotFoundException(movieId);

            return _mapper.MapMany<ApplicationCore.Entities.Movies.MovieShowtime, MovieShowtime>(showtimes);
        }
    }
}