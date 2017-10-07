using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Services;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps
{
    public abstract class GetWebApiDataBuildStep<TEntity> : IBuildStep<TEntity>
    {
        protected ILogger<GetWebApiDataBuildStep<TEntity>> Logger { get; }

        protected IParsableRequestData ParsableRequestData { get; }

        protected ISerializationService SerializationService { get; }

        protected IWebClient WebClient { get; }

        public abstract string Name { get; }

        public abstract int Position { get; }

        protected GetWebApiDataBuildStep(IWebClient webClient, IParsableRequestData parsableRequestData, ISerializationService serializationService, ILogger<GetWebApiDataBuildStep<TEntity>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ParsableRequestData = parsableRequestData ?? throw new ArgumentNullException(nameof(parsableRequestData));
            SerializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            WebClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        protected abstract Task<TEntity[]> ParseDataToEntities(string content);

        public virtual async Task<Result<TEntity[]>> BuildMany(TEntity[] entities)
        {
            using (Logger.BeginScope(nameof(BuildMany)))
            {
                Logger.LogDebug("Make request to api");

                var result = await WebClient.MakeRequestAsync(ParsableRequestData);

                Logger.LogDebug("Request result {@result}", result);

                if (!result.IsSuccess)
                    return Result.Failure<TEntity[]>(result.FailureReason);

                Logger.LogDebug("Parse received data to entities");

                var parsedEntities = await ParseDataToEntities(result.Value);

                Logger.LogDebug("Data parsed {@parsedEntities}", parsedEntities);

                return Result.Success(parsedEntities);
            }
        }
    }
}