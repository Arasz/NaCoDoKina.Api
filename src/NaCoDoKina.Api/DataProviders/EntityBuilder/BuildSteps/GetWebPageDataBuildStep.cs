using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Bindings;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps
{
    public abstract class GetWebPageDataBuildStep<TEntity> : IBuildStep<TEntity>
    {
        protected ILogger<GetWebPageDataBuildStep<TEntity>> Logger { get; }

        protected readonly IWebPageBinder<TEntity> WebPageBinder;

        public abstract string Name { get; }

        public abstract int Position { get; }

        public virtual async Task<Result<TEntity[]>> BuildMany(TEntity[] entities)
        {
            using (Logger.BeginScope(nameof(BuildMany)))
            {
                Logger.LogDebug("Start binding web page binding with binder {binder} for elements {@elements}", WebPageBinder.GetType().Name, entities);

                foreach (var entity in entities)
                {
                    using (Logger.BeginScope(entity))
                    {
                        Logger.LogDebug("Get web page url");

                        var url = await GetWebPageUrl(entity);

                        Logger.LogDebug("Binding page {url}", url);

                        await WebPageBinder.BindAsync(entity, url);

                        Logger.LogDebug("Entity binded");
                    }
                }

                Logger.LogDebug("All elements binded successfully");

                return Result.Success(entities);
            }
        }

        protected GetWebPageDataBuildStep(IWebPageBinder<TEntity> webPageBinder, ILogger<GetWebPageDataBuildStep<TEntity>> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            WebPageBinder = webPageBinder ?? throw new ArgumentNullException(nameof(webPageBinder));
        }

        /// <summary>
        /// Get web page url for entity 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns> Url for web page connected with entity </returns>
        protected abstract Task<string> GetWebPageUrl(TEntity entity);
    }
}