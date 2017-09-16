namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Web service url 
    /// </summary>
    public class ServiceUrl
    {
        /// <summary>
        /// Web service name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Web service url 
        /// </summary>
        public string Url { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Url)}: {Url}";
        }
    }
}