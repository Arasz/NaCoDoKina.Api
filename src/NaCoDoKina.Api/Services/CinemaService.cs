using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class CinemaService : ICinemaService
    {
        public Task<IEnumerable<Cinema>> GetNearestCinemasForMovie(long movieId, Location location)
        {
            throw new System.NotImplementedException();
        }
    }
}