using AutoMapper;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Models.Cinemas;
using NaCoDoKina.Api.Models.Movies;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IDisabledMovieService _disabledMovieService;
        private readonly ILogger<IMovieService> _logger;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaService _cinemaService;

        public MovieService(IMovieRepository movieRepository, IDisabledMovieService disabledMovieService,
            ICinemaService cinemaService, IRatingService ratingService, IMapper mapper, ILogger<IMovieService> logger)
        {
            _disabledMovieService = disabledMovieService ?? throw new ArgumentNullException(nameof(disabledMovieService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _cinemaService = cinemaService ?? throw new ArgumentNullException(nameof(cinemaService));
        }

        public async Task<ICollection<long>> GetAllMoviesAsync(SearchArea searchArea)
        {
            var cinemas = await _cinemaService.GetCinemasInSearchAreaAsync(searchArea);

            var movies = await GetMoviesPlayedInCinemas(cinemas);

            if (movies is null || !movies.Any())
                throw new MoviesNotFoundException(cinemas, searchArea);

            return movies;
        }

        private async Task<long[]> GetMoviesPlayedInCinemas(IEnumerable<Cinema> cinemas)
        {
            var currentDate = DateTime.Now;

            DateTime CalculateArrivalTime(Cinema cinema) => currentDate
                .Add(cinema.CinemaTravelInformation.Duration);

            var availableMoviesIds = new List<long>();
            foreach (var cinema in cinemas)
            {
                var arrivalTime = CalculateArrivalTime(cinema);
                var movies = await _movieRepository.GetMoviesForCinemaAsync(cinema.Id, arrivalTime);
                var notDisabledMovies = await _disabledMovieService.FilterDisabledMoviesForCurrentUserAsync(movies);

                availableMoviesIds.AddRange(notDisabledMovies);
            }

            var movieRatings = new List<(long MovieId, double Rating)>();
            foreach (var movieId in availableMoviesIds.Distinct())
            {
                var rating = await _ratingService.GetMovieRating(movieId);
                movieRatings.Add((movieId, rating));
            }

            return movieRatings
                .OrderByDescending(tuple => tuple.Rating)
                .Select(tuple => tuple.MovieId)
                .ToArray();
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);

            var isMovieDisabled = await _disabledMovieService.IsMovieDisabledForGivenUserAsync(id);

            if (movie is null || isMovieDisabled)
                throw new MovieNotFoundException(id);

            var mappedMovie = _mapper.Map<Movie>(movie);

            return mappedMovie;
        }

        public async Task DeleteMovieAsync(long id)
        {
            await _disabledMovieService.DisableMovieForCurrentUserAsync(id);
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            var movieDetails = await _movieRepository.GetMovieDetailsAsync(id);

            var isMovieDisabled = await _disabledMovieService.IsMovieDisabledForGivenUserAsync(id);

            if (movieDetails is null || isMovieDisabled)
                throw new MovieDetailsNotFoundException(id);

            var mappedDetails = _mapper.Map<MovieDetails>(movieDetails);

            try
            {
                var rating = await _ratingService.GetMovieRating(id);
                mappedDetails.Rating = rating;
            }
            catch (RatingNotFoundException e)
            {
                _logger.LogWarning("Rating for movie {MovieID} not found when getting a movie details. {@Exception}", id, e);
            }

            return mappedDetails;
        }

        public async Task<long> AddMovieAsync(Movie newMovie)
        {
            var entityMovie = _mapper.Map<Entities.Movies.Movie>(newMovie);

            var movieId = await _movieRepository.CreateMovieAsync(entityMovie);

            if (movieId == 0L)
                throw new MovieNotAddedException(newMovie);

            return movieId;
        }

        public async Task<long> AddMovieDetailsAsync(MovieDetails movieDetails)
        {
            var entityMovieDetails = _mapper.Map<Entities.Movies.MovieDetails>(movieDetails);

            var movieId = await _movieRepository.CreateMovieDetailsAsync(entityMovieDetails);

            if (movieId == 0L)
                throw new MovieDetailsNotAddedException(movieDetails);

            return movieId;
        }
    }
}