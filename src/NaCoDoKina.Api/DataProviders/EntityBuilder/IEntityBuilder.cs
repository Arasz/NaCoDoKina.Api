using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    /// <summary>
    /// Builds entity of given type from provided steps 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityBuilder<TEntity>
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
        /// Execute all build steps 
        /// </summary>
        /// <returns> Built entity </returns>
        Task<TEntity> Build();
    }
}