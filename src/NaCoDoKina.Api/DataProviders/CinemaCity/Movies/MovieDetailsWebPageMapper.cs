using AngleSharp;
using NaCoDoKina.Api.DataProviders.Parsers;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies
{
    public class MovieDetailsWebPageMapper : IWebPageMapper<Movie>
    {
        private readonly IBrowsingContext _browsingContext;
        private readonly CinemaCityMoviePageMappingSettings _settings;
        private readonly Dictionary<string, PropertyInfo> _movieDetailsProperties;

        public MovieDetailsWebPageMapper(IBrowsingContext browsingContext, CinemaCityMoviePageMappingSettings settings)
        {
            _browsingContext = browsingContext;
            _settings = settings;
            _movieDetailsProperties = typeof(MovieDetails)
                .GetProperties()
                .ToDictionary(info => info.Name, info => info);
        }

        public async Task MapAsync(string url, Movie mapped)
        {
            var document = await _browsingContext.OpenAsync(url);

            var movieDetails = mapped.Details;

            foreach (var propertySelectorMap in _settings.PropertySelectorMaps)
            {
                var property = _movieDetailsProperties[propertySelectorMap.PropertyName];

                var element = document.QuerySelector(propertySelectorMap.Selector);
                var elementText = element.TextContent;

                switch (property.PropertyType.Name)
                {
                    case nameof(DateTime):
                        if (DateTime.TryParse(elementText, out var parsedDate))
                        {
                            property.SetValue(movieDetails, parsedDate);
                        }
                        break;

                    default:
                        // string as default property type
                        property.SetValue(movieDetails, elementText);
                        break;
                }
            }
        }
    }
}