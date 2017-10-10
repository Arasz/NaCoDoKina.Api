using System;
using System.Threading.Tasks;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataProviders.Reviews
{
    public abstract class ReviewSearchBase<TEntity> : IReviewSearch<TEntity>
    {
        protected ReviewServicesSettings ReviewServicesSettings { get; }

        protected ILogger<ReviewSearchBase<TEntity>> Logger { get; }

        protected ReviewSearchBase(ReviewServicesSettings reviewServicesSettings, ILogger<ReviewSearchBase<TEntity>> logger)
        {
            ReviewServicesSettings = reviewServicesSettings;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public abstract Task<string> Search(TEntity entity);
    }
}