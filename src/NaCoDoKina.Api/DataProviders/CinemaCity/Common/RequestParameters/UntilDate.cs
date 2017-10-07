using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Extensions;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Common.RequestParameters
{
    public class UntilDate : RequestParameter
    {
        public UntilDate() : base(nameof(UntilDate), DateTime.Now.AddYears(1).ToChinaDate())
        {
        }
    }
}