using AngleSharp;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Results;
using Infrastructure.DataProviders.Bindings;
using Infrastructure.Settings;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Bindings
{
    public class CinemaCityMovieWebPageBinder : WebPageBinderBase<Movie>
    {
        private readonly CinemaNetworksSettings _settings;
        private readonly INodeBinder<MovieDetails> _nodeBinder;

        public CinemaCityMovieWebPageBinder(IBrowsingContext browsingContext, INodeBinder<MovieDetails> nodeBinder, CinemaNetworksSettings settings)
            : base(browsingContext)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _nodeBinder = nodeBinder ?? throw new ArgumentNullException(nameof(nodeBinder));
        }

        public override async Task<Result> BindAsync(Movie binded, string url)
        {
            var document = await BrowsingContext.OpenAsync(url);

            var movieDetails = binded.Details;

            return _nodeBinder.Bind(movieDetails, document, _settings.CinemaCityNetwork.MoviePageMappings);
        }
    }
}