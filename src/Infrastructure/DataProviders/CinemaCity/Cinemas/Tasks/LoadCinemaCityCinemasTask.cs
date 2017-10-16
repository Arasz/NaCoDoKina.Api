using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks
{
    public class LoadCinemaCityCinemasTask : EntitiesBuildTask<Cinema, EmptyContext>
    {
        private readonly ICinemaRepository _cinemaRepository;

        public LoadCinemaCityCinemasTask(ICinemaRepository cinemaRepository,
            IEntitiesBuilder<Cinema, EmptyContext> entitiesBuilder,
            TasksSettings settings,
            ILogger<LoadCinemaCityCinemasTask> logger)
            : base(entitiesBuilder, settings, logger)
        {
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        protected override async Task BuildEntities()
        {
            Results = (await EntitiesBuilder.BuildMany()).ToList();
            Logger.LogDebug("Built {BuiltCinemasCount} cinemas, first {@Cinema}", Results.Count, Results.First());
        }

        protected override async Task SaveResults()
        {
            await _cinemaRepository.CreateCinemasAsync(Results);
        }
    }
}