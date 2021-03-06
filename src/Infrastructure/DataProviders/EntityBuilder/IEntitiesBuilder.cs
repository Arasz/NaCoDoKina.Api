﻿using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.EntityBuilder
{
    /// <summary>
    /// Builds entities of given type from provided steps 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IEntitiesBuilder<TEntity, in TContext>
        where TContext : IEntityBuilderContext
    {
        /// <summary>
        /// Is entity fully built 
        /// </summary>
        bool Successful { get; }

        /// <summary>
        /// Build failure 
        /// </summary>
        BuildFailure BuildFailure { get; }

        /// <summary>
        /// Build steps 
        /// </summary>
        IReadOnlyList<IBuildStep<TEntity, TContext>> BuildSteps { get; }

        /// <summary>
        /// Currently executed build step 
        /// </summary>
        int CurrentStep { get; }

        /// <summary>
        /// Execute all build steps for entities collection 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> BuildMany(CancellationToken cancellationToken = default(CancellationToken),
            TContext parameters = default(TContext));
    }
}