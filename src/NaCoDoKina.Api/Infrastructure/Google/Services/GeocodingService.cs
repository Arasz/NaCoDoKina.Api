using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public class GeocodingService : BaseGoogleService<GeocodingApiRequest, GeocodingApiResponse>, IGeocodingService
    {
        public GeocodingService(GoogleServiceDependencies<GeocodingApiRequest> googleServiceDependencies)
            : base(googleServiceDependencies)
        {
        }

        protected override string BaseUrl { get; } = "https://maps.googleapis.com/maps/api/geocode/";

        public Task<GeocodingApiResponse> GeocodeAsync(GeocodingApiRequest geocodingApiRequest)
            => MakeRequest(geocodingApiRequest);
    }
}