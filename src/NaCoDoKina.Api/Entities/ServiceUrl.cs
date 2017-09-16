namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Url to service 
    /// </summary>
    public class ServiceUrl
    {
        public ServiceUrl()
        {
        }

        public ServiceUrl(string name, string url)
        {
            Name = name;
            Url = url;
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Url)}: {Url}";
        }
    }
}