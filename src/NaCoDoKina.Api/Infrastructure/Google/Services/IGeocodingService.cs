using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    /// <summary>
    /// Client for google geocoding service 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro"/>
    public interface IGeocodingService
    {
        /// <summary>
        /// Makes request to google geocoding api 
        /// </summary>
        /// <param name="geocodingApiRequest"></param>
        /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro"/>
        /// <returns> Google api response </returns>
        Task<GeocodingApiResponse> GeocodeAsync(GeocodingApiRequest geocodingApiRequest);
    }
}