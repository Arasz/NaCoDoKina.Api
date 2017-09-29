using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace NaCoDoKina.Api.Infrastructure.Extensions
{
    public static class MapperExtensions
    {
        public static IEnumerable<TDestination> MapMany<TSource, TDestination>(this IMapper mapper,
            IEnumerable<TSource> sources)
        {
            return sources.Select(source => mapper.Map<TDestination>(source));
        }
    }
}