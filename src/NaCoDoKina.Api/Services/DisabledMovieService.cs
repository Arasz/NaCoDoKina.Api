using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class DisabledMovieService : IDisabledMovieService
    {
        private readonly IDisabledMovieRepository _disabledMovieRepository;
        private readonly IUserService _userService;

        public DisabledMovieService(IDisabledMovieRepository disabledMovieRepository, IUserService userService)
        {
            _disabledMovieRepository = disabledMovieRepository ?? throw new ArgumentNullException(nameof(disabledMovieRepository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task DisableMovieForCurrentUserAsync(long movieId)
        {
            var userId = _userService.GetCurrentUserId();

            if (userId != 0)
                await _disabledMovieRepository.CreateDisabledMovieAsync(movieId, userId);
        }

        public async Task<ICollection<long>> FilterDisabledMoviesForCurrentUserAsync(IEnumerable<long> moviesIds)
        {
            var userId = _userService.GetCurrentUserId();

            if (userId != 0)
            {
                var disabledMovies = await _disabledMovieRepository.GetDisabledMoviesIdsForUserAsync(userId);
                return moviesIds
                    .Except(disabledMovies)
                    .ToArray();
            }

            return moviesIds.ToArray();
        }

        public async Task<bool> IsMovieDisabledForGivenUserAsync(long movieId)
        {
            var userId = _userService.GetCurrentUserId();

            if (userId != 0)
                return await _disabledMovieRepository.IsMovieDisabledAsync(movieId, userId);

            return false;
        }
    }
}