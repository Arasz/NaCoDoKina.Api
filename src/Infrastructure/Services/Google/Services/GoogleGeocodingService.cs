using System.Threading.Tasks;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Response;

namespace Infrastructure.Services.Google.Services
{
    public class GoogleGeocodingService : GoogleServiceBase<GeocodingApiRequest, GeocodingApiResponse>, IGoogleGeocodingService
    {
        public GoogleGeocodingService(GoogleServiceDependencies<GeocodingApiRequest> googleServiceDependencies)
            : base(googleServiceDependencies)
        {
        }

        protected override string BaseUrl { get; } = "https://maps.googleapis.com/maps/api/geocode/";

        public Task<GeocodingApiResponse> GeocodeAsync(GeocodingApiRequest geocodingApiRequest)
            => MakeRequest(geocodingApiRequest);
    }
}