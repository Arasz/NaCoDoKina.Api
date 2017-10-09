using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks
{
    public class CinemaCityCinemasTask : TaskBase
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IEntitiesBuilder<Cinema, EmptyContext> _entitiesBuilder;

        public CinemaCityCinemasTask(ICinemaRepository cinemaRepository, IEntitiesBuilder<Cinema, EmptyContext> entitiesBuilder, TasksSettings settings) : base(settings)
        {
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
        }

        public override async Task Execute()
        {
            var cinemas = await _entitiesBuilder.BuildMany();

            await _cinemaRepository.CreateCinemasAsync(cinemas);
        }
    }
}