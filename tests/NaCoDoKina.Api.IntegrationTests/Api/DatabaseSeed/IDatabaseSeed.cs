using System;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed
{
    /// <inheritdoc/>
    /// <summary>
    /// Responsible for database initialization 
    /// </summary>
    public interface IDatabaseSeed : IDisposable
    {
        /// <summary>
        /// Seeds database with data 
        /// </summary>
        /// <returns></returns>
        void Seed();
    }
}