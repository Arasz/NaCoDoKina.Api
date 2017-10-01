using ApplicationCore.Results;
using NaCoDoKina.Api.Services;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder
{
    public abstract class GetDataBuildStep<TEntity> : IBuildStep<TEntity>
    {
        protected IParsableRequestData ParsableRequestData { get; };

        protected ISerializationService SerializationService { get; }

        protected IWebClient WebClient { get; }

        public abstract string Name { get; }

        public abstract int Position { get; }

        protected GetDataBuildStep(IWebClient webClient, IParsableRequestData parsableRequestData, ISerializationService serializationService)
        {
            ParsableRequestData = parsableRequestData ?? throw new ArgumentNullException(nameof(parsableRequestData));
            SerializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            WebClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        protected abstract Task<TEntity[]> BuildModelsFromResponseContent(string content);

        public virtual async Task<Result<TEntity[]>> BuildMany(TEntity[] entities)
        {
            var result = await WebClient.MakeRequestAsync(ParsableRequestData);

            if (!result.IsSuccess)
                return Result.Failure<TEntity[]>(result.FailureReason);

            var parsedEntities = await BuildModelsFromResponseContent(result.Value);
            return Result.Success(parsedEntities);
        }
    }
}