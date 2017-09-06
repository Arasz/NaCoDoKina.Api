using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    /// <summary>
    /// Client for google directions service 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    public interface IDirectionsService
    {
        /// <summary>
        /// Returns directions for travel described by api request 
        /// </summary>
        /// <param name="directionsApiRequest"> Request </param>
        /// <returns> Directions </returns>
        Task<DirectionsApiResponse> GetDirections(DirectionsApiRequest directionsApiRequest);
    }
}