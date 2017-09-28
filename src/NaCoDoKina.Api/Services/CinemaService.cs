using AutoMapper;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Models.Cinemas;
using NaCoDoKina.Api.Models.Travel;
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

        public async Task<IEnumerable<Cinema>> GetCinemasPlayingMovieInSearchArea(long movieId, SearchArea searchArea)
        {
            var allCinemasForMovie = (await _cinemaRepository.GetAllCinemasForMovieAsync(movieId))
                .ToArray();

            if (allCinemasForMovie is null || !allCinemasForMovie.Any())
                throw new CinemasNotFoundException(movieId, searchArea);

            return await FindNearestCinemasAsync(searchArea, allCinemasForMovie);
        }

        private async Task<IEnumerable<TravelInformation>> GetTravelInformationForCinemas(SearchArea searchArea, IEnumerable<Cinema> allCinemaModels)
        {
            var travelPlans = allCinemaModels
                .Select(cinema => new TravelPlan(searchArea.Center, cinema.Location))
                .ToArray();

            var travelInformation = new List<TravelInformation>(travelPlans.Length);
            foreach (var travelPlan in travelPlans)
            {
                var information = await _travelService.CalculateInformationForTravelAsync(travelPlan);
                travelInformation.Add(information);
            }

            return travelInformation;
        }

        private Cinema[] MapCinemas(IEnumerable<Entities.Cinemas.Cinema> cinemas)
        {
            var allCinemaModels = cinemas
                .Select(_mapper.Map<Cinema>)
                .ToArray();
            return allCinemaModels;
        }

        public async Task<IEnumerable<Cinema>> GetNearestCinemasAsync(SearchArea searchArea)
        {
            var allCinemas = (await _cinemaRepository.GetAllCinemas())
                .ToArray();

            if (allCinemas is null || !allCinemas.Any())
                throw new CinemasNotFoundException(searchArea);

            return await FindNearestCinemasAsync(searchArea, allCinemas);
        }

        private async Task<IEnumerable<Cinema>> FindNearestCinemasAsync(SearchArea searchArea, IEnumerable<Entities.Cinemas.Cinema> allCinemas)
        {
            var allCinemaModels = MapCinemas(allCinemas);

            var travelInformationForCinemas = await GetTravelInformationForCinemas(searchArea, allCinemaModels);

            var nearestCinemas = allCinemaModels
                .Join(travelInformationForCinemas, cinema => cinema.Location,
                    information => information.TravelPlan.Destination,
                    (cinema, information) =>
                    {
                        cinema.CinemaTravelInformation = information;
                        return (Cinema: cinema, Distance: information.Distance);
                    })
                .Where(tuple => tuple.Distance <= searchArea.Radius)
                .Select(tuple => tuple.Cinema);

            return nearestCinemas;
        }

        public async Task<Cinema> AddCinemaAsync(Cinema cinema)
        {
            var entityCinema = _mapper.Map<Entities.Cinemas.Cinema>(cinema);
            entityCinema = await _cinemaRepository.AddCinema(entityCinema);
            return _mapper.Map<Cinema>(entityCinema);
        }

        public async Task<Cinema> GetCinemaAsync(long id)
        {
            var cinema = await _cinemaRepository.GetCinemaAsync(id);

            if (cinema is null)
                throw new CinemaNotFoundException(id);

            return _mapper.Map<Cinema>(cinema);
        }

        public async Task<Cinema> GetCinemaAsync(string name)
        {
            var cinema = await _cinemaRepository.GetCinemaAsync(name);

            if (cinema is null)
                throw new CinemaNotFoundException(name);

            return _mapper.Map<Cinema>(cinema);
        }
    }
}