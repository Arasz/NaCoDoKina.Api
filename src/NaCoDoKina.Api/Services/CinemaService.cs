using AutoMapper;
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
    public class CinemaService : ICinemaService
    {
        private readonly ITravelService _travelService;
        private readonly ICinemaRepository _cinemaRepository;
        private readonly ILogger<ICinemaService> _logger;
        private readonly IMapper _mapper;

        public CinemaService(ICinemaRepository cinemaRepository, ITravelService travelService, ILogger<ICinemaService> logger, IMapper mapper)
        {
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<Cinema>> GetNearestCinemasForMovieAsync(long movieId, SearchArea searchArea)
        {
            var allCinemasForMovie = (await _cinemaRepository.GetAllCinemasForMovie(movieId))
                .ToArray();

            if (allCinemasForMovie is null || !allCinemasForMovie.Any())
                throw new CinemasNotFoundException(movieId, searchArea);

            var allCinemaModels = allCinemasForMovie
                .Select(_mapper.Map<Cinema>)
                .ToArray();

            var travelInformationTasks = allCinemaModels
                .Select(cinema => new TravelPlan(searchArea.Center, cinema.Location))
                .Select(plan => _travelService.CalculateInformationForTravelAsync(plan));

            var travelInformationForCinemas = await Task.WhenAll(travelInformationTasks);

            var nearestCinemas = allCinemaModels
                .Join(travelInformationForCinemas, cinema => cinema.Location,
                    information => information.TravelPlan.Destination,
                    (cinema, information) => (Cinema: cinema, Distance: information.Distance))
                .Where(tuple => tuple.Distance <= searchArea.Radius)
                .Select(tuple => tuple.Cinema);

            return nearestCinemas;
        }

        public Task<IEnumerable<Cinema>> GetNearestCinemasAsync(SearchArea searchArea)
        {
            throw new System.NotImplementedException();
        }

        public Task<Cinema> AddCinemaAsync(Cinema cinema)
        {
            throw new System.NotImplementedException();
        }
    }
}