using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class DisabledMovieRepository : IDisabledMovieRepository
    {
        private readonly ICacheManager<HashSet<long>> _cacheManager;
        private readonly ApplicationContext _applicationContext;

        public DisabledMovieRepository(ApplicationContext applicationContext, ICacheManager<HashSet<long>> cacheManager)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<DisabledMovie> CreateDisabledMovieAsync(long movieId, long userId)
        {
            var disabledMovie = new DisabledMovie(movieId, userId);
            _applicationContext.DisabledMovies.Add(disabledMovie);
            await _applicationContext.SaveChangesAsync();

            return disabledMovie;
        }

        public async Task<IEnumerable<long>> GetDisabledMoviesIdsForUserAsync(long userId)
        {
            var disabledMovies = _cacheManager.Get(userId.ToString());

            if (disabledMovies is null)
            {
                var result = await _applicationContext.DisabledMovies
                    .Where(m => m.UserId == userId)
                    .Select(m => m.MovieId)
                    .ToArrayAsync();

                disabledMovies = result.ToHashSet();

                _cacheManager.Put(userId.ToString(), disabledMovies);
            }
            return disabledMovies;
        }

        public async Task<bool> DeleteDisabledMovieAsync(long movieId, long userId)
        {
            var toDelete = await FindDisabledMovie(movieId, userId)
                .SingleOrDefaultAsync();

            if (toDelete is null)
                return false;

            _cacheManager.Remove(userId.ToString());

            _applicationContext.DisabledMovies.Remove(toDelete);
            await _applicationContext.SaveChangesAsync();
            return true;
        }

        private IQueryable<DisabledMovie> FindDisabledMovie(long movieId, long userId)
        {
            return _applicationContext.DisabledMovies
                .Where(movie => movie.MovieId == movieId)
                .Where(movie => movie.UserId == userId);
        }

        public async Task<bool> IsMovieDisabledAsync(long movieId, long userId)
        {
            var disabledMovies = _cacheManager.Get(userId.ToString());

            if (disabledMovies != null)
            {
                return disabledMovies.Contains(movieId);
            }

            return await FindDisabledMovie(movieId, userId)
                .AnyAsync();
        }
    }
}