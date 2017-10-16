using FluentValidation;
using NaCoDoKina.Api.DataContracts.Authentication;

namespace NaCoDoKina.Api.Validators
{
    public class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(credentials => credentials.UserName)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(credentials => credentials.Password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(40);
        }
    }
}