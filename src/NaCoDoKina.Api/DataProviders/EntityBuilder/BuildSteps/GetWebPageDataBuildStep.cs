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

        public virtual bool Enabled => true;

        public virtual async Task<Result<TEntity[]>> BuildMany(TEntity[] entities)
        {
            using (Logger.BeginScope(nameof(BuildMany)))
            using (Logger.BeginScope(WebPageBinder.GetType().Name))
            {
                Logger.LogDebug("Start binding to web pages for {ElementsCount} elements of {EntityType}", entities.Length, typeof(TEntity));

                foreach (var entity in entities)
                {
                    using (Logger.BeginScope(entity))
                    {
                        Logger.LogDebug("Get web page url");

                        var url = await GetWebPageUrl(entity);

                        Logger.LogDebug("Binding page under {Url} to entity {EntityType}", url, typeof(TEntity));

                        var bindingResult = await WebPageBinder.BindAsync(entity, url);

                        if (bindingResult.IsSuccess)
                        {
                            Logger.LogDebug("Entity {EntityType} binded", typeof(TEntity));
                        }
                        else
                        {
                            Logger.LogWarning("Entity {EntityType} binding error {BindingError}", typeof(TEntity), bindingResult.FailureReason);
                        }
                    }
                }

                Logger.LogDebug("Binding completed");

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