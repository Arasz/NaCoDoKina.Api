using FluentValidation;
using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            //https://stackoverflow.com/questions/7780981/how-to-validate-latitude-and-longitude

            RuleFor(location => location.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(location => location.Latitude)
                .InclusiveBetween(-90, 90);
        }
    }
}