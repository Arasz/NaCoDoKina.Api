﻿using ApplicationCore.Results;
using System.Collections.Generic;
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
        /// Execute part of work to build entities 
        /// </summary>
        /// <param name="entities"> Partially built entities </param>
        /// <returns> Partially built entities </returns>
        Task<Result<IEnumerable<TEntity>>> BuildMany(IEnumerable<TEntity> entities);
    }
}