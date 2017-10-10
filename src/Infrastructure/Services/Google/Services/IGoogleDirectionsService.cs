using System.Threading.Tasks;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Directions.Response;

namespace Infrastructure.Services.Google.Services
{
    /// <summary>
    /// Client for google directions service 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    public interface IGoogleDirectionsService
    {
        /// <summary>
        /// Returns directions for travel described by api request 
        /// </summary>
        /// <param name="directionsApiRequest"> Request </param>
        /// <returns> Directions </returns>
        Task<DirectionsApiResponse> GetDirections(DirectionsApiRequest directionsApiRequest);
    }
}