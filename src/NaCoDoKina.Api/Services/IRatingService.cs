using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public interface IRatingService
    {
        Task<double> GetMovieRating(long movieId);

        Task SetMovieRating(long movieId, double movieRating);
    }
}