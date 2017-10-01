using ApplicationCore.Results;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    /// <summary>
    /// Marker interface 
    /// </summary>
    public interface IBuildStep
    {
    }

    /// <summary>
    /// Entity build step 
    /// </summary>
    public interface IBuildStep<TEntity> : IBuildStep
    {
        /// <summary>
        /// Build step name 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Position in build queue 
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Execute part of work to build entity 
        /// </summary>
        /// <param name="entity"> Partially build entity </param>
        /// <returns> Partially build entity </returns>
        Task<Result<TEntity>> Build(TEntity entity);
    }
}