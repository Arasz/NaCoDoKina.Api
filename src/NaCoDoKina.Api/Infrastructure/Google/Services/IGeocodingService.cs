using NaCoDoKina.Api.Infrastructure.Google.DataContract;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Google
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
        /// <param name="address"> Place address </param>
        /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro"/>
        /// <returns> Google api response </returns>
        Task<GeocodingApiResponse> GeocodeAsync(string address);
    }
}