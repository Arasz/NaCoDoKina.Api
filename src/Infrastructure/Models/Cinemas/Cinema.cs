using Infrastructure.Models.Travel;

namespace Infrastructure.Models.Cinemas
{
    /// <summary>
    /// Information about cinema 
    /// </summary>
    public class Cinema
    {
        /// <summary>
        /// Cinema id 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Cinema name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City where cinema is located 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Cinema address 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Cinema site url 
        /// </summary>
        public string CinemaUrl { get; set; }

        /// <summary>
        /// Network name to which cinema belongs 
        /// </summary>
        public string NetworkName { get; set; }

        /// <summary>
        /// Cinema location 
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Information about travel from user location to cinema 
        /// </summary>
        public TravelInformation CinemaTravelInformation { get; set; }

        protected bool Equals(Cinema other)
        {
            return Id == other.Id &&
                   string.Equals(Name, other.Name) &&
                   string.Equals(City, other.City) &&
                   string.Equals(Address, other.Address) &&
                   string.Equals(CinemaUrl, other.CinemaUrl) &&
                   string.Equals(NetworkName, other.NetworkName) &&
                   Equals(Location, other.Location);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cinema)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CinemaUrl != null ? CinemaUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NetworkName != null ? NetworkName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Location != null ? Location.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}