using Infrastructure.DataProviders.Requests;

namespace Infrastructure.DataProviders.CinemaCity.Common.RequestParameters
{
    public class LanguageTag : RequestParameter
    {
        public LanguageTag() : base(nameof(LanguageTag), "pl_PL")
        {
        }
    }
}