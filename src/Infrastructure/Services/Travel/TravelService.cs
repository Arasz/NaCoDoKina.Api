using AutoMapper;
using CacheManager.Core;
using Infrastructure.Extensions;
using Infrastructure.Models;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using Infrastructure.Services.Google.Exceptions;
using Infrastructure.Services.Google.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Travel
{
    public class TravelService : ITravelService
    {
        private readonly IGoogleDirectionsService _googleDirectionsService;
        private readonly IGoogleGeocodingService _googleGeocodingService;
        private readonly ITravelInformationEstimator _travelInformationEstimator;
        private readonly IMapper _mapper;
        private readonly ILogger<ITravelService> _logger;
        private readonly ICacheManager<TravelInformation> _travelInfoCacheManager;

        public TravelService(IGoogleDirectionsService googleDirectionsService, IGoogleGeocodingService googleGeocodingService, ITravelInformationEstimator travelInformationEstimator,
            IMapper mapper, ILogger<ITravelService> logger, ICacheManager<TravelInformation> travelInfoCacheManager)
        {
            _googleDirectionsService = googleDirectionsService;
            _googleGeocodingService = googleGeocodingService;
            _travelInformationEstimator = travelInformationEstimator ?? throw new ArgumentNullException(nameof(travelInformationEstimator));
            _mapper = mapper;
            _logger = logger;
            _travelInfoCacheManager = travelInfoCacheManager ?? throw new ArgumentNullException(nameof(travelInfoCacheManager));
        }

        public async Task<TravelInformation> GetInformationForTravelAsync(TravelPlan travelPlan)
        {
            var cacheKey = $"{travelPlan.Origin.ToCacheKey()}-{travelPlan.Destination.ToCacheKey()}";
            var travelInfo = _travelInfoCacheManager.Get(cacheKey);

            if (travelInfo is null)
            {
                var request = _mapper.Map<DirectionsApiRequest>(travelPlan);

                try
                {
                    var response = await _googleDirectionsService.GetDirections(request);

                    travelInfo = response.Routes
                        .SelectMany(route => route.Legs)
                        .Select(leg => new TravelInformation(travelPlan, leg.Distance.Value,
                            TimeSpan.FromSeconds(leg.Duration.Value)))
                        .Max();
                }
                catch (GoogleApiException exception)
                {
                    _logger.LogError("Failure during travel time calculation {@Exception}.", exception);
                    travelInfo = EstimateTravelInformation(travelPlan);
                }
                finally
                {
                    _travelInfoCacheManager.Put(cacheKey, travelInfo);
                }
            }

            return travelInfo;
        }

        private TravelInformation EstimateTravelInformation(TravelPlan travelPlan)
        {
            var estimated = _travelInformationEstimator.Estimate(travelPlan);
            _logger.LogWarning("Travel information {@Information} for plan {@Plan} was estimated", estimated, travelPlan);
            return estimated;
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
            catch (GoogleApiException exception)
            {
                _logger.LogError("Failure during address location {@Exception}.", exception);
                return null;
            }
        }
    }
}