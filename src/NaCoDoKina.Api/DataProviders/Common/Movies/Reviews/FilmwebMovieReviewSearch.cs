using AngleSharp;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Bindings;
using NaCoDoKina.Api.DataProviders.Reviews;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Common.Movies.Reviews
{
    public class FilmwebMovieReviewSearch : ReviewSearchBase<Movie>
    {
        private readonly ReviewServicesSettings _searchServiceSettings;
        private readonly ICollectionNodeBinder<MovieSearchResult> _nodeBinder;
        private readonly IBrowsingContext _browsingContext;
        private readonly SearchQuery _searchQuery;
        private readonly ReviewService _reviewService;

        public FilmwebMovieReviewSearch(IBrowsingContext browsingContext, ReviewServicesSettings reviewServicesSettings,
            ICollectionNodeBinder<MovieSearchResult> nodeBinder, ILogger<FilmwebMovieReviewSearch> logger)
            : base(reviewServicesSettings, logger)
        {
            _searchServiceSettings = reviewServicesSettings ?? throw new ArgumentNullException(nameof(reviewServicesSettings));
            _nodeBinder = nodeBinder ?? throw new ArgumentNullException(nameof(nodeBinder));

            _browsingContext = browsingContext ?? throw new ArgumentNullException(nameof(browsingContext));
            _reviewService = reviewServicesSettings.Filmweb ?? throw new ArgumentNullException(nameof(reviewServicesSettings.Filmweb));
            _searchQuery = _reviewService.SearchQuery ?? throw new ArgumentNullException(nameof(reviewServicesSettings.Filmweb.SearchQuery));
        }

        public class MovieSearchResult
        {
            public string RelativeUrl { get; set; }

            public string ToUrl()
            {
                return RelativeUrl;
            }
        }

        public override async Task<string> Search(Movie entity)
        {
            using (Logger.BeginScope(nameof(Search)))
            using (Logger.BeginScope(entity))
            {
                string ReplaceQueryValue(QueryParameter queryParameter)
                {
                    if (entity.HasProperty(queryParameter.PropertyName))
                    {
                        var newValue = entity.GetPropertyValue<string>(queryParameter.PropertyName);

                        Logger.LogDebug("Replacing query parameter {@queryParameter} value with {@newValue}", queryParameter, newValue);

                        return newValue;
                    }

                    return queryParameter.Value;
                }

                var query = _searchQuery.Build(ReplaceQueryValue);

                Logger.LogDebug("Built query {query} from {@searchQuery}", query, _searchQuery);

                var queryUrl = _reviewService.BuildQueryUrl(query);

                Logger.LogDebug("Built full query url {queryUrl} with {@reviewServiceSettings", queryUrl, _reviewService);
                Logger.LogDebug("Open page under url {queryUrl}", queryUrl);

                var document = await _browsingContext.OpenAsync(queryUrl);

                Logger.LogDebug("Opened page under url {queryUrl}", queryUrl);

                var queryResultsSelectors = _searchQuery.QueryResults;

                Logger.LogDebug("Bind query results with selector {@selectors}", queryResultsSelectors);

                var results = _nodeBinder.Bind(document, queryResultsSelectors.ResultElementsSelectors, queryResultsSelectors.ResultsCollectionSelector);

                Logger.LogDebug("Results binded {@bindedResults}", results);

                var selectedResult = results.First();

                var parsedUrl = $"{_searchServiceSettings.Filmweb.BaseUrl}{selectedResult.ToUrl()}";

                Logger.LogDebug("Final query result selected {@result} wit url {url}", selectedResult, parsedUrl);

                return parsedUrl;
            }
        }
    }
}