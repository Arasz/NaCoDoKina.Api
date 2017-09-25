using System;
using Microsoft.EntityFrameworkCore;

namespace NaCoDoKina.Api.Repository.Database
{
    /// <inheritdoc/>
    /// <summary>
    /// Scope in which application context exist 
    /// </summary>
    public class DbContextScope<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }

        public DbContextScope(InMemoryDatabaseScope databaseScope)
        {
            var options = new DbContextOptionsBuilder<TDbContext>()
                .EnableSensitiveDataLogging()
                .UseSqlite(databaseScope.Connection)
                .Options;
            DbContext = Activator.CreateInstance(typeof(TDbContext), options) as TDbContext;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}