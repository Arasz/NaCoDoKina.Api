using ApplicationCore.Entities.Movies;
using Infrastructure.DataProviders.Common.Movies.Mappings;
using Infrastructure.DataProviders.Common.Movies.Reviews;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Common.Movies.BuildSteps
{
    public class GetFilmwebMovieReviewBuildStep : GetWebPageDataBuildStep<Movie, EmptyContext>
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