using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Exceptions;
using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Tasks
{
    public abstract class EntitiesBuildTask<TEntity, TBuildContext> : TaskBase
        where TBuildContext : IEntityBuilderContext
    {
        protected readonly IEntitiesBuilder<TEntity, TBuildContext> EntitiesBuilder;

        protected List<TEntity> Results { get; set; }

        protected EntitiesBuildTask(IEntitiesBuilder<TEntity, TBuildContext> entitiesBuilder,
            TasksSettings settings,
            ILogger<EntitiesBuildTask<TEntity, TBuildContext>> logger)
            : base(settings, logger)
        {
            EntitiesBuilder = entitiesBuilder;
        }

        /// <summary>
        /// Part of execution where entities are build with builder 
        /// </summary>
        /// <returns></returns>
        protected abstract Task BuildEntities();

        /// <summary>
        /// Part of execution where entities are saved to database 
        /// </summary>
        /// <returns></returns>
        protected abstract Task SaveResults();

        public override async Task Execute()
        {
            using (Logger.BeginScope(GetType().Name))
            {
                Logger.LogInformation("Start task execution and begin entities building");
                await BuildEntities();
                Logger.LogInformation("Entities building ended, check if was successful");
                ThrowIfBuildNotSuccessful();
                Logger.LogInformation("Entities built successfully, saving results");
                await SaveResults();
                Logger.LogInformation("Result saved, task execution ended");
            }
        }

        protected virtual void ThrowIfBuildNotSuccessful()
        {
            if (!EntitiesBuilder.Successful)
            {
                Logger.LogError("Build failed with failure {@BuildFailure}", EntitiesBuilder.BuildFailure);
                throw new TaskExecutionException(EntitiesBuilder.BuildFailure);
            }
        }
    }
}