using System.Collections.Generic;

namespace NaCoDoKina.Api.Entities
{
    public class CinemaNetwork : Entity
    {
        public string Name { get; set; }

        public IEnumerable<Cinema> Cinemas { get; set; }
    }
}