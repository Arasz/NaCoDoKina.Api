namespace Infrastructure.DataProviders.CinemaCity.Common
{
    /// <summary>
    /// Cinema City data api service response type 
    /// </summary>
    public class CinemaCityResponse<TBody>
    {
        /// <summary>
        /// Response body 
        /// </summary>
        public TBody Body { get; set; }
    }
}