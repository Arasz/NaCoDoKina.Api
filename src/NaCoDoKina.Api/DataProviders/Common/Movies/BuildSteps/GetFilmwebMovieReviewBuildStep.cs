using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Common.Movies.Mappings;
using NaCoDoKina.Api.DataProviders.Common.Movies.Reviews;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.Entities.Movies;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Common.Movies.BuildSteps
{
    public class GetFilmwebMovieReviewBuildStep : GetWebPageDataBuildStep<Movie>
    {
        private readonly FilmwebMovieReviewSearch _filmwebMovieReviewSearch;

        public GetFilmwebMovieReviewBuildStep(FilmwebReviewWebPageBinder filmwebReviewWebPageBinder, FilmwebMovieReviewSearch filmwebMovieReviewSearch, ILogger<GetFilmwebMovieReviewBuildStep> logger)
            : base(filmwebReviewWebPageBinder, logger)
        {
            _filmwebMovieReviewSearch = filmwebMovieReviewSearch ?? throw new ArgumentNullException(nameof(filmwebMovieReviewSearch));
        }

        public override string Name => "Get movie review";

        public override int Position => 15;

        public override bool Enabled => false;

        protected override async Task<string> GetWebPageUrl(Movie entity)
        {
            using (Logger.BeginScope(nameof(GetWebPageUrl)))
            {
                return await _filmwebMovieReviewSearch.Search(entity);
            }
        }
    }
}