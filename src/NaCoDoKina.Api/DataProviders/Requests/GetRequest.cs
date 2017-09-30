namespace NaCoDoKina.Api.DataProviders.Requests
{
    public class GetRequest : Request
    {
        public string QueryString { get; set; }

        public GetRequest(string baseUrl) : base(baseUrl)
        {
        }

        public override string BuildUrl() => $"{BaseUrl}?{QueryString}";
    }
}