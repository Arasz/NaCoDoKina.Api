using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    public class EntityBuilder<TEntity> : IEntityBuilder<TEntity>
        where TEntity : Entities.Entity, new()
    {
        private readonly ILogger<EntityBuilder<TEntity>> _logger;
        public bool Successful { get; private set; }

        public BuildFailure BuildFailure { get; private set; }

        public IReadOnlyList<IBuildStep<TEntity>> BuildSteps { get; }

        public int CurrentStep { get; private set; }

        public EntityBuilder(IEnumerable<IBuildStep<TEntity>> buildSteps, ILogger<EntityBuilder<TEntity>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BuildSteps = buildSteps.ToImmutableList() ?? throw new ArgumentNullException(nameof(buildSteps));
        }

        public async Task<TEntity> Build()
        {
            using (_logger.BeginScope(nameof(EntityBuilder<TEntity>)))
            {
                var entity = new TEntity();

                foreach (var buildStep in BuildSteps.OrderBy(step => step.Position))
                {
                    CurrentStep++;

                    _logger.LogDebug("Build step {@step} number {current}", buildStep, CurrentStep);

                    var result = await buildStep.Build(entity);

                    if (result.IsSuccess)
                        entity = result.Value;
                    else
                    {
                        Successful = false;
                        BuildFailure = new BuildFailure(result.FailureReason, CurrentStep);
                        _logger.LogDebug("Build step {@step} number {current} failed with failure {@failure}", buildStep, CurrentStep, result.FailureReason);
                        return entity;
                    }
                }

                Successful = true;
                return entity;
            }
        }
    }
}