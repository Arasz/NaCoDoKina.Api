using AutoMapper;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.Exceptions;
using NaCoDoKina.Api.Infrastructure.Services.Google.Services;

namespace NaCoDoKina.Api.Services
{
    public class TravelService : ITravelService
    {
        private readonly IGoogleDirectionsService _googleDirectionsService;
        private readonly IGoogleGeocodingService _googleGeocodingService;
        private readonly IMapper _mapper;
        private readonly ILogger<ITravelService> _logger;

        public TravelService(IGoogleDirectionsService googleDirectionsService, IGoogleGeocodingService googleGeocodingService, IMapper mapper, ILogger<ITravelService> logger)
        {
            _googleDirectionsService = googleDirectionsService;
            _googleGeocodingService = googleGeocodingService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TravelInformation> CalculateInformationForTravelAsync(TravelPlan travelPlan)
        {
            var request = _mapper.Map<DirectionsApiRequest>(travelPlan);

            try
            {
                var response = await _googleDirectionsService.GetDirections(request);

                return response.Routes
                    .SelectMany(route => route.Legs)
                    .Select(leg => new TravelInformation(travelPlan, leg.Distance.Value, TimeSpan.FromSeconds(leg.Duration.Value)))
                    .Max();
            }
            catch (GoogleApiException exception) when (exception.Status != GoogleApiStatus.Unspecifed)
            {
                _logger.LogError("Error during travel time calculation {@Exception}.", exception);
                return null;
            }
            catch (GoogleApiException)
            {
                return null;
            }
        }

        public async Task<Location> TranslateAddressToLocationAsync(string address)
        {
            var request = _mapper.Map<GeocodingApiRequest>(address);

            try
            {
                var response = await _googleGeocodingService.GeocodeAsync(request);

                var locationFromApi = response.Results.FirstOrDefault()?
                    .Geometry
                    .Location;

                return _mapper.Map<Location>(locationFromApi);
            }
            catch (GoogleApiException exception) when (exception.Status != GoogleApiStatus.Unspecifed)
            {
                _logger.LogError("Error during address location {@Exception}.", exception);
                return null;
            }
            catch (GoogleApiException)
            {
                return null;
            }
        }
    }
}