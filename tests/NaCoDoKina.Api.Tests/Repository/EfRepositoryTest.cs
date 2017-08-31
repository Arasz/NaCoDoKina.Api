using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System;

namespace NaCoDoKina.Api.Repository
{
    public class InMemeorySqliteDatabaseScope<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        private TDbContext DbContext { get; }

        private readonly SqliteConnection _sqliteConnection;

        public InMemeorySqliteDatabaseScope()
        {
            _sqliteConnection = new SqliteConnection("DataSource=:memory:");
            _sqliteConnection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<TDbContext>()
                .UseSqlite(_sqliteConnection)
                .Options;

            DbContext = Activator.CreateInstance(typeof(TDbContext), dbContextOptions) as TDbContext;
        }

        public void Dispose()
        {
            _sqliteConnection.Close();
        }
    }

    public class EfRepositoryTest<TEntity>
        where TEntity : Entity
    {
        protected IRepository<TEntity> CreateRepositoryUnderTest(ApplicationContext context)
        {
            return new EfGenericRepository<TEntity>(context);
        }
    }
}