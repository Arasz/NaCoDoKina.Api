namespace NaCoDoKina.Api.Infrastructure.Settings
{
    public class CinemaNetworksSettings : MultiElementSettingsBase<CinemaNetwork>
    {
        public CinemaNetwork CinemaCityNetwork { get; set; }
    }
}