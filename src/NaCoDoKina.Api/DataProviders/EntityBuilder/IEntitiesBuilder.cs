using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    /// <summary>
    /// Builds entities of given type from provided steps 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntitiesBuilder<TEntity>
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
        IReadOnlyList<IBuildStep<TEntity>> BuildSteps { get; }

        /// <summary>
        /// Currently executed build step 
        /// </summary>
        int CurrentStep { get; }

        /// <summary>
        /// Execute all build steps for entities collection 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> BuildMany(CancellationToken cancellationToken = default(CancellationToken));
    }
}