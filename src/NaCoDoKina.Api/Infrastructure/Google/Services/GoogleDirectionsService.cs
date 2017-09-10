using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public class GoogleDirectionsService : GoogleServiceBase<DirectionsApiRequest, DirectionsApiResponse>, IGoogleDirectionsService
    {
        public GoogleDirectionsService(GoogleServiceDependencies<DirectionsApiRequest> googleServiceDependencies)
            : base(googleServiceDependencies)
        {
        }

        protected override string BaseUrl { get; } = "https://maps.googleapis.com/maps/api/directions/";

        public Task<DirectionsApiResponse> GetDirections(DirectionsApiRequest directionsApiRequest)
            => MakeRequest(directionsApiRequest);
    }
}