using System.Linq;

namespace NaCoDoKina.Api.Infrastructure.Settings
{
    public class CinemaNetworksSettings
    {
        public CinemaNetwork CinemaCityNetwork { get; set; }

        private CinemaNetwork[] _allNetworks;

        public CinemaNetwork[] GetAllNetworks()
        {
            var type = GetType();

            if (_allNetworks is null)
                _allNetworks = type.GetProperties()
                .Where(info => info.PropertyType == typeof(CinemaNetwork))
                .Select(info => info.GetValue(this))
                .Cast<CinemaNetwork>()
                .ToArray();

            return _allNetworks;
        }
    }
}