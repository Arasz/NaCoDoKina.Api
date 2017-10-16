using ApplicationCore.Entities;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.EntityBuilder
{
    public class EntitiesBuilder<TEntity, TContext> : IEntitiesBuilder<TEntity, TContext>
        where TEntity : Entity, new()
        where TContext : IEntityBuilderContext
    {
        private readonly ILogger<EntitiesBuilder<TEntity, TContext>> _logger;
        public bool Successful { get; private set; }

        public BuildFailure BuildFailure { get; private set; }

        public IReadOnlyList<IBuildStep<TEntity, TContext>> BuildSteps { get; }

        public int CurrentStep { get; private set; }

        public EntitiesBuilder(IEnumerable<IBuildStep<TEntity, TContext>> buildSteps, ILogger<EntitiesBuilder<TEntity, TContext>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            BuildSteps = buildSteps.ToImmutableList() ?? throw new ArgumentNullException(nameof(buildSteps));
        }

        public async Task<IEnumerable<TEntity>> BuildMany(CancellationToken cancellationToken = default(CancellationToken), TContext parameters = default(TContext))
        {
            using (_logger.BeginScope(nameof(EntitiesBuilder<TEntity, TContext>)))
            {
                var entities = Array.Empty<TEntity>();

                foreach (var buildStep in BuildSteps.OrderBy(step => step.Position))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _logger.LogDebug("Cancellation requested");
                        return entities;
                    }

                    CurrentStep++;

                    if (!buildStep.Enabled)
                    {
                        _logger.LogDebug("Build step {StepName} number {CurrentStep} on position {Position} is disabled", buildStep.Name, CurrentStep, buildStep.Position);
                        continue;
                    }

                    _logger.LogDebug("Build step {StepName} number {CurrentStep} on position {Position} will run", buildStep.Name, CurrentStep, buildStep.Position);

                    var result = await buildStep.BuildMany(entities, parameters);

                    if (result.IsSuccess)
                        entities = result.Value;
                    else
                    {
                        Successful = false;
                        BuildFailure = new BuildFailure(result.FailureReason, CurrentStep, BuildSteps.Count, buildStep.Name);
                        return entities;
                    }
                }

                Successful = true;
                return entities;
            }
        }
    }
}