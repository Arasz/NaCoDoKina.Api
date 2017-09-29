using FluentValidation;
using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Validators
{
    public class SearchAreaValidator : AbstractValidator<SearchArea>
    {
        public SearchAreaValidator()
        {
            RuleFor(area => area.Radius)
                .ExclusiveBetween(0, 1000001);

            RuleFor(area => area.Center)
                .NotEmpty()
                .SetValidator(new LocationValidator());
        }
    }
}