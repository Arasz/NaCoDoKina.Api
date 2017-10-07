using AngleSharp;
using NaCoDoKina.Api.DataProviders.Bindings;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Bindings
{
    public class MovieWebPageBinder : WebPageBinderBase<Movie>
    {
        private readonly CinemaNetworksSettings _settings;
        private readonly IDocumentBinder<MovieDetails> _documentBinder;

        public MovieWebPageBinder(IBrowsingContext browsingContext, CinemaNetworksSettings settings, IDocumentBinder<MovieDetails> documentBinder) : base(browsingContext)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _documentBinder = documentBinder ?? throw new ArgumentNullException(nameof(documentBinder));
        }

        public override async Task BindAsync(Movie binded, string url)
        {
            var document = await BrowsingContext.OpenAsync(url);

            var movieDetails = binded.Details;

            _documentBinder.Bind(movieDetails, document, _settings.CinemaCityNetwork.MoviePageMappings);
        }
    }
}