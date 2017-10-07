using ApplicationCore.Results;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps
{
    /// <summary>
    /// Entity build step 
    /// </summary>
    public interface IBuildStep<TEntity, in TContext>
        where TContext : IEntityBuilderContext
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
        /// Is build step enabled 
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Execute part of work to build entities 
        /// </summary>
        /// <param name="entities"> Partially built entities </param>
        /// <param name="buildContext"></param>
        /// <returns> Partially built entities </returns>
        Task<Result<TEntity[]>> BuildMany(TEntity[] entities, TContext buildContext);
    }
}