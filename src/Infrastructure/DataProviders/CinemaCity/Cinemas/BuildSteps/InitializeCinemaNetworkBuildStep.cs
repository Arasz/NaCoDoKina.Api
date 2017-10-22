using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Repositories;
using ApplicationCore.Results;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Settings;
using System;
using System.Threading.Tasks;
using Infrastructure.Settings.CinemaNetwork;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    public class InitializeCinemaNetworkBuildStep : IBuildStep<Cinema, EmptyContext>
    {
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;
        public string Name => "Initialize cinema network build step";

        public int Position => 2;

        public bool Enabled => true;

        public async Task<Result<Cinema[]>> BuildManyAsync(Cinema[] entities, EmptyContext context)
        {
            var cinemaNetwork = await _cinemaNetworkRepository.GetByNameAsync(_cinemaNetworksSettings.CinemaCityNetwork.Name);

            if (cinemaNetwork is null)
                return Result.Failure<Cinema[]>("Can not found cinema network for cinema");

            string AppendToBaseUrl(string url) => $"{cinemaNetwork.CinemaNetworkUrl}{url}";

            foreach (var cinema in entities)
            {
                cinema.CinemaNetwork = cinemaNetwork;
                cinema.CinemaUrl = AppendToBaseUrl(cinema.CinemaUrl);
            }

            return Result.Success(entities);
        }

        public InitializeCinemaNetworkBuildStep(ICinemaNetworkRepository cinemaNetworkRepository, CinemaNetworksSettings cinemaNetworksSettings)
        {
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
        }
    }
}