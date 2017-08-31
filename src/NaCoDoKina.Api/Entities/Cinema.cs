namespace NaCoDoKina.Api.Entities
{
    public class Cinema : Entity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public Location Location { get; set; }

        public CinemaNetwork CinemaNetwork { get; set; }
    }
}