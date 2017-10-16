namespace Infrastructure.Exceptions
{
    public class CinemaNotFoundException : NaCoDoKinaApiException
    {
        public CinemaNotFoundException(long cinemaId)
            : base($"Cinema with id {cinemaId} not found")
        {
        }

        public CinemaNotFoundException(string cinemaName)
            : base($"Cinema {cinemaName} not found")
        {
        }
    }
}