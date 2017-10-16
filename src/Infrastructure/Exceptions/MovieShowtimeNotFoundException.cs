namespace Infrastructure.Exceptions
{
    public class MovieShowtimeNotFoundException : NaCoDoKinaApiException
    {
        public MovieShowtimeNotFoundException(long movieId)
            : base($"Movie {movieId} is not played in any cinema")
        {
        }

        public MovieShowtimeNotFoundException(long movieId, long cinemaId)
            : base($"Movie {movieId} is not played in cinema {cinemaId}")
        {
        }
    }
}