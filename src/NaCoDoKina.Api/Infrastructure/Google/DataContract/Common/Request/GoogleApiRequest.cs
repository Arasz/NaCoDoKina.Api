namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request
{
    public abstract class GoogleApiRequest
    {
        public string Key { get; set; }

        

        protected GoogleApiRequest(string key)
        {
            Key = key;
        }

        protected GoogleApiRequest()
        {
        }
    }
}