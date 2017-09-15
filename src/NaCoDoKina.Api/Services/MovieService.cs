﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<IMovieService> _logger;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ICinemaService _cinemaService;

        public MovieService(IMovieRepository movieRepository, ICinemaService cinemaService, IRatingService ratingService, IMapper mapper, ILogger<IMovieService> logger)
        {
            _logger = logger;
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _cinemaService = cinemaService ?? throw new ArgumentNullException(nameof(cinemaService));
        }

        public async Task<IEnumerable<long>> GetAllMoviesAsync(SearchArea searchArea)
        {
            var nearestCinemas = await _cinemaService.GetNearestCinemasAsync(searchArea);

            var moviesPlayedInCinemas = await GetMoviesPlayedInCinemas(nearestCinemas);

            if (moviesPlayedInCinemas is null || !moviesPlayedInCinemas.Any())
                throw new MoviesNotFoundException(nearestCinemas, searchArea);

            return moviesPlayedInCinemas;
        }

        private async Task<IEnumerable<long>> GetMoviesPlayedInCinemas(IEnumerable<Cinema> cinemas)
        {
            var currentDate = DateTime.Now;

            DateTime DateAfterTravel(Cinema cinema)
            {
                return currentDate.Add(cinema.CinemaTravelInformation.Duration);
            }

            var getPlayedMoviesTasks = cinemas
                .Select(cinema => _movieRepository.GetMoviesPlayedInCinemaAsync(cinema.Id, DateAfterTravel(cinema)));

            var availableMovies = (await Task.WhenAll(getPlayedMoviesTasks))
                .SelectMany(movieId => movieId)
                .Distinct();

            async Task<(long MovieId, double Rating)> GetMovieRating(long movieId)
            {
                return (movieId, await _ratingService.GetMovieRating(movieId));
            }

            var getMovieRatingsForMoviesTasks = availableMovies
                .Select(GetMovieRating);

            var movieRatings = await Task.WhenAll(getMovieRatingsForMoviesTasks);

            return movieRatings
                .OrderByDescending(tuple => tuple.Rating)
                .Select(tuple => tuple.MovieId);
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);

            if (movie is null)
                throw new MovieNotFoundException(id);

            var mappedMovie = _mapper.Map<Movie>(movie);

            return mappedMovie;
        }

        public async Task DeleteMovieAsync(long id)
        {
            var deleted = await _movieRepository.SoftDeleteMovieAsync(id);

            if (!deleted)
                throw new MovieNotFoundException(id);
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            var movieDetails = await _movieRepository.GetMovieDetailsAsync(id);

            if (movieDetails is null)
                throw new MovieDetailsNotFoundException(id);

            var mappedDetails = _mapper.Map<MovieDetails>(movieDetails);

            try
            {
                var rating = await _ratingService.GetMovieRating(id);
                mappedDetails.Rating = rating;
            }
            catch (RatingNotFoundException e)
            {
                _logger.LogWarning("Rating for movie {id} not found when getting a movie details. {@e}", id, e);
            }

            return mappedDetails;
        }

        public async Task<long> AddMovieAsync(Movie newMovie)
        {
            var entityMovie = _mapper.Map<Entities.Movie>(newMovie);

            var movieId = await _movieRepository.AddMovieAsync(entityMovie);

            if (movieId == 0L)
                throw new MovieNotAddedException(newMovie);

            return movieId;
        }

        public async Task<long> AddMovieDetailsAsync(MovieDetails movieDetails)
        {
            var entityMovieDetails = _mapper.Map<Entities.MovieDetails>(movieDetails);

            var movieId = await _movieRepository.AddMovieDetailsAsync(entityMovieDetails);

            if (movieId == 0L)
                throw new MovieDetailsNotAddedException(movieDetails);

            return movieId;
        }
    }
}