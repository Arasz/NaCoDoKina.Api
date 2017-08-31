using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class InMemeorySqliteDatabaseScope<TDbContext> : IDisposable
        where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }

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

    public class EfRepositoryTest
    {
        private IEnumerable<IRepository<Entity>> _repositories;

        protected ApplicationContextSeed DatabaseSeed { get; } = new ApplicationContextSeed(new NullLogger<ApplicationContextSeed>());

        protected InMemeorySqliteDatabaseScope<ApplicationContext> DatabaseScope => new InMemeorySqliteDatabaseScope<ApplicationContext>();

        protected IEnumerable<IRepository<Entity>> CreateRepositoriesUnderTest(ApplicationContext context)
        {
            if (_repositories != null)
                return _repositories;

            var entitiesAssembly = Assembly.GetAssembly(typeof(Entity));
            _repositories = entitiesAssembly.GetExportedTypes()
                .Where(type => type.Namespace.Contains("Entities"))
                .Where(type => type.IsAssignableFrom(typeof(Entity)))
                .Where(type => type != typeof(Entity))
                .Select(type => CreateRepositoryInstance(context, type))
                .ToArray();

            return _repositories;
        }

        private static IRepository<Entity> CreateRepositoryInstance(ApplicationContext context, Type type) =>
            Activator.CreateInstance(typeof(EfGenericRepository<>).MakeGenericType(type), context) as IRepository<Entity>;
    }

    public class GetByIdAsync : EfRepositoryTest
    {
        [Fact]
        public async Task Should_return_entity_object_with_correct_id()
        {
            using (var scope = DatabaseScope)
            {
                //Arrange
                await DatabaseSeed.SeedAsync(scope.DbContext);
                var repositories = CreateRepositoriesUnderTest(scope.DbContext);
                var id = 1;

                //Act
                foreach (var repository in repositories)
                {
                    var entity = await repository.GetByIdAsync(id);
                    //Assert
                    entity.Should().NotBeNull();
                    entity.Id.Should().Be(id);
                }
            }
        }

        [Fact]
        public async Task Should_return_null_when_id_was_not_found()
        {
            using (var scope = DatabaseScope)
            {
                //Arrange
                await DatabaseSeed.SeedAsync(scope.DbContext);
                var repositories = CreateRepositoriesUnderTest(scope.DbContext);
                var id = 1;

                //Act
                foreach (var repository in repositories)
                {
                    var entity = await repository.GetByIdAsync(id);
                    //Assert
                    entity.Should().BeNull();
                }
            }
        }
    }
}