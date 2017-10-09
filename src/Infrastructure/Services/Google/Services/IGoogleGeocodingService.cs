using System.Threading.Tasks;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Response;

namespace Infrastructure.Services.Google.Services
{
    /// <summary>
    /// Client for google geocoding service 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro"/>
    public interface IGoogleGeocodingService
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