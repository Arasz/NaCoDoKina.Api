using System;
using System.Collections.Generic;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed;

namespace NaCoDoKina.Api.IntegrationTests.Api.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private readonly IEnumerable<IDatabaseSeed> _databaseSeeds;

        public DatabaseFixture(IEnumerable<IDatabaseSeed> databaseSeeds)
        {
            _databaseSeeds = databaseSeeds;
        }

        public void CreateDatabase()
        {
            foreach (var databaseSeed in _databaseSeeds)
            {
                databaseSeed.Seed();
            }
        }

        public void Dispose()
        {
            foreach (var databaseSeed in _databaseSeeds)
            {
                databaseSeed.Dispose();
            }
        }
    }
}