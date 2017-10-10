namespace Infrastructure.DataProviders.Requests
{
    public class Request
    {
        public string Url { get; }

        public Request(string url)
        {
            Url = url;
        }

        public virtual string BuildUrl() => Url;
    }
}