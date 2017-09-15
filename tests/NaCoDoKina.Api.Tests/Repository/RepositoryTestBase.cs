using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NaCoDoKina.Api.Data;
using System;

namespace NaCoDoKina.Api.Repository
{
    public abstract class RepositoryTestBase<TRepository>
    {
        protected TRepository RepositoryUnderTest { get; set; }

        protected Mock<ILogger<TRepository>> LoggerMock { get; }

        protected RepositoryTestBase()
        {
            LoggerMock = new Mock<ILogger<TRepository>>();
        }

        /// <inheritdoc/>
        /// <summary>
        /// Scope in which in memory database exist 
        /// </summary>
        public class InMemoryDatabaseScope : IDisposable
        {
            public SqliteConnection Connection { get; }

            public InMemoryDatabaseScope()
            {
                Connection = new SqliteConnection("DataSource=:memory:");
                Connection.Open();
            }

            public void Dispose()
            {
                Connection.Close();
            }
        }

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
                    .UseSqlite(databaseScope.Connection)
                    .Options;
                ApplicationContext = new ApplicationContext(options);
            }

            public void Dispose()
            {
                ApplicationContext.Dispose();
            }
        }

        /// <summary>
        /// Ensures that database schema is created 
        /// </summary>
        /// <param name="databaseScope"> Database scope </param>
        protected void EnsureCreated(InMemoryDatabaseScope databaseScope)
        {
            using (var contextScope = new TestDatabaseContextScope(databaseScope))
            {
                contextScope.ApplicationContext.Database.EnsureCreated();
            }
        }
    }
}