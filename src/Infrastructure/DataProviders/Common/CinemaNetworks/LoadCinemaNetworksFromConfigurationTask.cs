using ApplicationCore.Repositories;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings;
using Infrastructure.Settings.Tasks;
using System;
using System.Threading.Tasks;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace Infrastructure.DataProviders.Common.CinemaNetworks
{
    public class LoadCinemaNetworksFromConfigurationTask : TaskBase
    {
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;

        public override async Task Execute()
        {
            var allNetworks = _cinemaNetworksSettings.GetAllElements();

            foreach (var cinemaNetwork in allNetworks)
            {
                var exist = await _cinemaNetworkRepository.ExistAsync(cinemaNetwork.Name);
                if (!exist)
                {
                    var network = new CinemaNetwork
                    {
                        Name = cinemaNetwork.Name,
                        CinemaNetworkUrl = cinemaNetwork.Url
                    };
                    await _cinemaNetworkRepository.CreateAsync(network);
                }
            }
        }

        public LoadCinemaNetworksFromConfigurationTask(ICinemaNetworkRepository cinemaNetworkRepository, CinemaNetworksSettings cinemaNetworksSettings, TasksSettings settings)
            : base(settings)
        {
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
        }
    }
}