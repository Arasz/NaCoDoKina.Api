using System.Threading.Tasks;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Directions.Response;

namespace Infrastructure.Services.Google.Services
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