using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps
{
    public abstract class GetWebApiDataBuildStep<TEntity, TContext> : IBuildStep<TEntity, TContext>
        where TContext : IEntityBuilderContext
    {
        protected ILogger<GetWebApiDataBuildStep<TEntity, TContext>> Logger { get; }

        protected IParsableRequestData ParsableRequestData { get; }

        protected ISerializationService SerializationService { get; }

        protected IWebClient WebClient { get; }

        public abstract string Name { get; }

        public abstract int Position { get; }

        public virtual bool Enabled => true;

        protected GetWebApiDataBuildStep(IWebClient webClient, IParsableRequestData parsableRequestData, ISerializationService serializationService, ILogger<GetWebApiDataBuildStep<TEntity, TContext>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ParsableRequestData = parsableRequestData ?? throw new ArgumentNullException(nameof(parsableRequestData));
            SerializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            WebClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        /// <summary>
        /// Parses data received from web api to entities 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task<TEntity[]> ParseDataToEntities(string content, TContext context);

        /// <summary>
        /// Creates dynamic request parameters 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual IRequestParameter[] CreateRequestParameters(TEntity[] entities, TContext context)
        {
            return Array.Empty<IRequestParameter>();
        }

        public virtual async Task<Result<TEntity[]>> BuildMany(TEntity[] entities, TContext context)
        {
            using (Logger.BeginScope(nameof(BuildMany)))
            {
                var requestParameters = CreateRequestParameters(entities, context);

                Logger.LogDebug("Created {ParametersCount} request parameters {@RequestParameters}", requestParameters.Length, requestParameters.Cast<object>());

                Logger.LogDebug("Making request with request data {@ParsableRequestData}", ParsableRequestData);

                var result = await WebClient.MakeRequestAsync(ParsableRequestData, requestParameters);

                Logger.LogDebug("Request result {@Result}", result);

                if (!result.IsSuccess)
                    return Result.Failure<TEntity[]>(result.FailureReason);

                Logger.LogDebug("Parse received data to entities");

                var parsedEntities = await ParseDataToEntities(result.Value, context);

                Logger.LogDebug("Data parsed {@ParsedEntities}", parsedEntities);

                return Result.Success(parsedEntities);
            }
        }
    }
}