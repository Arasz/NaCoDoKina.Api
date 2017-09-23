using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using System;

namespace NaCoDoKina.Api.Repository
{
    /// <inheritdoc/>
    /// <summary>
    /// Scope in which application context exist 
    /// </summary>
    public class TestDatabaseContextScope : IDisposable
    {
        public ApplicationContext ApplicationContext { get; }

        public TestDatabaseContextScope(InMemoryDatabaseScope databaseScope)
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .EnableSensitiveDataLogging()
                .UseSqlite(databaseScope.Connection)
                .Options;
            ApplicationContext = new ApplicationContext(options);
        }

        public void Dispose()
        {
            ApplicationContext.Dispose();
        }
    }
}