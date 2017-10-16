using System;
using System.Threading.Tasks;
using AngleSharp;
using ApplicationCore.Results;

namespace Infrastructure.DataProviders.Bindings
{
    public abstract class WebPageBinderBase<TBinded> : IWebPageBinder<TBinded>
    {
        protected readonly IBrowsingContext BrowsingContext;

        protected WebPageBinderBase(IBrowsingContext browsingContext)
        {
            BrowsingContext = browsingContext ?? throw new ArgumentNullException(nameof(browsingContext));
        }

        public abstract Task<Result> BindAsync(TBinded binded, string url);
    }
}