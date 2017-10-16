using Infrastructure.Settings.Common;

namespace Infrastructure.Settings.CinemaNetwork
{
    public class CinemaNetworksSettings : MultiElementSettingsBase<CinemaNetwork>
    {
        public CinemaNetwork CinemaCityNetwork { get; set; }
    }
}