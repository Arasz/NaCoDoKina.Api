using ApplicationCore.Repositories;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.CinemaNetwork;
using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
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
            using (Logger.BeginScope(GetType().Name))
            {
                Logger.LogInformation("Start task execution");

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
                        Logger.LogDebug("New cinema network in configuration {@CinemaNetwork}", network);
                        var id = await _cinemaNetworkRepository.CreateAsync(network);
                        Logger.LogDebug("Cinema network created with id {@CinemaNetworkId}", id);
                    }
                    else
                    {
                        Logger.LogDebug("Cinema network {@CinemaNetwork} exist", cinemaNetwork);
                    }
                }

                Logger.LogInformation("Task executed successfully");
            }
        }

        public LoadCinemaNetworksFromConfigurationTask(ICinemaNetworkRepository cinemaNetworkRepository,
            CinemaNetworksSettings cinemaNetworksSettings,
            TasksSettings settings,
            ILogger<LoadCinemaNetworksFromConfigurationTask> logger)
            : base(settings, logger)
        {
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
        }
    }
}