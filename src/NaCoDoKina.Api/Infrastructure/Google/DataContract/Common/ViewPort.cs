using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding
{
    public class ViewPort
    {
        public Location Northeast { get; set; }

        public Location Southwest { get; set; }
    }
}