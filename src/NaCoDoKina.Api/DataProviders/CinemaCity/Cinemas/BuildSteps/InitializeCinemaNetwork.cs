using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Cinemas;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    public class InitializeCinemaNetworkBuildStep : IBuildStep<Cinema>
    {
        public string Name => "Initialize build step";

        public int Position => 2;

        public Task<Result<Cinema>> Build(Cinema entity)
        {
            throw new System.NotImplementedException();
        }
    }
}