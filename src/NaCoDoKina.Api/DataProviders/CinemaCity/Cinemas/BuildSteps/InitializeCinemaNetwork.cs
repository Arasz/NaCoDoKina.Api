using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    public class InitializeCinemaNetworkBuildStep : IBuildStep<Cinema>
    {
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;
        public string Name => "Initialize cinema network build step";

        public int Position => 2;

        public async Task<Result<Cinema[]>> BuildMany(Cinema[] entities)
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