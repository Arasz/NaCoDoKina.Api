﻿namespace DataProviders
{
    public class Request
    {
        public string BaseUrl { get; }

        public Request(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public virtual string BuildUrl() => BaseUrl;
    }
}