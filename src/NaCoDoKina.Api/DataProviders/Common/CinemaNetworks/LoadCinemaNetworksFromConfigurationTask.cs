using AutoMapper;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Common.CinemaNetworks
{
    public class LoadCinemaNetworksFromConfigurationTask : TaskBase
    {
        private readonly IMapper _mapper;
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
                    var network = _mapper.Map<Entities.Cinemas.CinemaNetwork>(cinemaNetwork);
                    await _cinemaNetworkRepository.CreateAsync(network);
                }
            }
        }

        public LoadCinemaNetworksFromConfigurationTask(ICinemaNetworkRepository cinemaNetworkRepository, IMapper mapper, CinemaNetworksSettings cinemaNetworksSettings, TasksSettings settings) : base(settings)
        {
            _mapper = mapper;
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
        }
    }
}