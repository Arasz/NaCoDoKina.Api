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
            var cinemas = await _cinemaRepository.GetAllCinemasForMovieAsync(movieId);

            return await MapAndFilter(searchArea, cinemas);
        }

        private async Task<IEnumerable<Cinema>> MapAndFilter(SearchArea searchArea, IEnumerable<Entities.Cinemas.Cinema> cinemas)
        {
            var mappedCinemas = MapCinemas(cinemas);

            EnsureCinemasFound(searchArea, mappedCinemas);

            var cinemasInSearchArea = await ExcludeOutsideCinemas(searchArea, mappedCinemas);

            return cinemasInSearchArea;
        }

        private static void EnsureCinemasFound(SearchArea searchArea, Cinema[] mappedCinemas)
        {
            if (mappedCinemas is null || !mappedCinemas.Any())
                throw new CinemasNotFoundException(searchArea);
        }

        private async Task<IEnumerable<TravelInformation>> GetTravelInformationForCinemasAsync(SearchArea searchArea, Cinema[] cinemas)
        {
            var travelPlans = cinemas
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
            var mappedCinemas = cinemas
                ?.Select(_mapper.Map<Cinema>)
                .ToArray();
            return mappedCinemas;
        }

        public async Task<IEnumerable<Cinema>> GetCinemasInSearchAreaAsync(SearchArea searchArea)
        {
            var cinemas = await _cinemaRepository.GetAllCinemas();

            return await MapAndFilter(searchArea, cinemas);
        }

        private async Task<IEnumerable<Cinema>> ExcludeOutsideCinemas(SearchArea searchArea, Cinema[] cinemas)
        {
            var travelInformationForCinemas = await GetTravelInformationForCinemasAsync(searchArea, cinemas);

            var nearestCinemas = cinemas
                .Join(travelInformationForCinemas, cinema => cinema.Location,
                    information => information.TravelPlan.Destination,
                    (cinema, information) =>
                    {
                        cinema.CinemaTravelInformation = information;
                        return (Cinema: cinema, Distance: information.Distance);
                    })
                .Where(tuple => tuple.Distance <= searchArea.Radius)
                .Select(tuple => tuple.Cinema)
                .ToArray();

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