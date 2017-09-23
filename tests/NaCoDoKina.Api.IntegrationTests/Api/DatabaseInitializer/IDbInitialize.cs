using System.Threading.Tasks;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer
{
    /// <summary>
    /// Responsible for database initialization 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbInitialize<out TDbContext>
    {
        TDbContext DbContext { get; }

        Task InitializeAsync();
    }
}