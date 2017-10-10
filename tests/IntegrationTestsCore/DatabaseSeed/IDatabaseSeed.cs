using System;

namespace IntegrationTestsCore.DatabaseSeed
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