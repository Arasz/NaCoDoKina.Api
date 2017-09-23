using AutoMapper;
using System.Collections.Generic;

namespace NaCoDoKina.Api.Infrastructure.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IEnumerable<TSource> sources,
            IMapper mapper)
        {
            return mapper.MapMany<TSource, TDestination>(sources);
        }
    }
}