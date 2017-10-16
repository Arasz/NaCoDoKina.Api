using FluentValidation;
using NaCoDoKina.Api.DataContracts.Authentication;

namespace NaCoDoKina.Api.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        public RegisterUserValidator()
        {
            RuleFor(registerUser => registerUser.UserName)
                .NotEmpty()
                .MaximumLength(40);

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(registerUser => registerUser.Password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(40);
        }
    }
}