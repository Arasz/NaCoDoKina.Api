using AngleSharp;
using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.Bindings;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Common.Movies.Mappings
{
    public class FilmwebReviewWebPageBinder : WebPageBinderBase<Movie>
    {
        private readonly INodeBinder<ReviewLink> _reviewNodeBinder;
        private readonly ReviewServicesSettings _reviewServicesSettings;

        public FilmwebReviewWebPageBinder(
            IBrowsingContext browsingContext,
            INodeBinder<ReviewLink> reviewNodeBinder,
            ReviewServicesSettings reviewServicesSettings)
            : base(browsingContext)
        {
            _reviewNodeBinder = reviewNodeBinder;
            _reviewServicesSettings = reviewServicesSettings ?? throw new ArgumentNullException(nameof(reviewServicesSettings));
        }

        public override async Task<Result> BindAsync(Movie binded, string url)
        {
            var document = await BrowsingContext.OpenAsync(url);

            if (binded.Details.MovieReviews is null)
                binded.Details.MovieReviews = new List<ReviewLink>();

            var newReview = new ReviewLink
            {
                Name = _reviewServicesSettings.Filmweb.Name,
                Url = url
            };

            binded.Details.MovieReviews.Add(newReview);

            return _reviewNodeBinder.Bind(newReview, document, _reviewServicesSettings.Filmweb.ReviewBindingMappings);
        }
    }
}