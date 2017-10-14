using ApplicationCore.Repositories;
using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Models;
using Infrastructure.Models.Cinemas;
using Infrastructure.Models.Travel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
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

        public async Task<ICollection<Cinema>> GetCinemasPlayingMovieInSearchArea(long movieId, SearchArea searchArea)
        {
            var cinemas = await _cinemaRepository.GetAllCinemasForMovieAsync(movieId);

            return await MapAndFilter(searchArea, cinemas);
        }

        private async Task<ICollection<Cinema>> MapAndFilter(SearchArea searchArea, IEnumerable<ApplicationCore.Entities.Cinemas.Cinema> cinemas)
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
                var travelInfo = await _travelService.GetInformationForTravelAsync(travelPlan);
                travelInformation.Add(travelInfo);
            }

            return travelInformation;
        }

        private Cinema[] MapCinemas(IEnumerable<ApplicationCore.Entities.Cinemas.Cinema> cinemas)
        {
            var mappedCinemas = cinemas
                ?.Select(_mapper.Map<Cinema>)
                .ToArray();
            return mappedCinemas;
        }

        public async Task<ICollection<Cinema>> GetCinemasInSearchAreaAsync(SearchArea searchArea)
        {
            var cinemas = searchArea.City.IsNullOrEmpty() ?
                await _cinemaRepository.GetAllCinemas()
                :
                await _cinemaRepository.GetCinemasByCityAsync(searchArea.City);

            return await MapAndFilter(searchArea, cinemas);
        }

        private async Task<ICollection<Cinema>> ExcludeOutsideCinemas(SearchArea searchArea, Cinema[] cinemas)
        {
            var travelInformationForCinemas = await GetTravelInformationForCinemasAsync(searchArea, cinemas);

            var nearestCinemas = cinemas
                .Join(travelInformationForCinemas,
                    cinema => cinema.Location,
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
            var entityCinema = _mapper.Map<ApplicationCore.Entities.Cinemas.Cinema>(cinema);
            entityCinema = await _cinemaRepository.CreateCinemaAsync(entityCinema);
            return _mapper.Map<Cinema>(entityCinema);
        }

        public async Task<Cinema> GetCinemaAsync(long id)
        {
            var cinema = await _cinemaRepository.GetCinemaByIdAsync(id);

            if (cinema is null)
                throw new CinemaNotFoundException(id);

            return _mapper.Map<Cinema>(cinema);
        }

        public async Task<Cinema> GetCinemaAsync(string name)
        {
            var cinema = await _cinemaRepository.GetCinemaByNameAsync(name);

            if (cinema is null)
                throw new CinemaNotFoundException(name);

            return _mapper.Map<Cinema>(cinema);
        }
    }
}