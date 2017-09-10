using NaCoDoKina.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public interface ICinemaRepository
    {
        Task<IEnumerable<Cinema>> GetAllCinemasForMovie(long movieId);

        Task<IEnumerable<Cinema>> GetAllCinemas();

        Task<Cinema> AddCinema(Cinema cinema);
    }
}