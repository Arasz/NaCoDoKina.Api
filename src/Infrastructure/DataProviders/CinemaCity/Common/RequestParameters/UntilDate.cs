using Infrastructure.DataProviders.Requests;
using Infrastructure.Extensions;
using System;

namespace Infrastructure.DataProviders.CinemaCity.Common.RequestParameters
{
    public class UntilDate : RequestParameter
    {
        public UntilDate() : base(nameof(UntilDate), DateTime.Now.AddYears(1).ToChinaDate())
        {
        }
    }
}