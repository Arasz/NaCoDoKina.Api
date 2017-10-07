using NaCoDoKina.Api.DataProviders.Requests;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Common.RequestParameters
{
    public class LanguageTag : RequestParameter
    {
        public LanguageTag() : base(nameof(LanguageTag), "pl_PL")
        {
        }
    }
}