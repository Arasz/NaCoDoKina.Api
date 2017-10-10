namespace NaCoDoKina.Api.DataContracts.Cinemas
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

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(NetworkName)}: {NetworkName}";
        }
    }
}