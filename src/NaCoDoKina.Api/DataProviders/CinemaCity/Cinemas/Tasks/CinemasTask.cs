using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.Tasks
{
    public class CinemasTask : TaskBase
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IEntitiesBuilder<Cinema> _entitiesBuilder;

        public CinemasTask(ICinemaRepository cinemaRepository, IEntitiesBuilder<Cinema> entitiesBuilder, TasksSettings settings) : base(settings)
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