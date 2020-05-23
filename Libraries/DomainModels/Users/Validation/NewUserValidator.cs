using FluentValidation;

namespace Helpdesk.DomainModels.Users.Validation
{
    public class NewUserValidator : AbstractValidator<NewUser>
    {
        public NewUserValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(64);
            RuleFor(r => r.Surname).NotEmpty().MaximumLength(64);
            RuleFor(r => r.Username).NotEmpty().MaximumLength(128);
            RuleFor(r => r.Alias).NotEmpty().MaximumLength(128);
            RuleFor(r => r.Email).EmailAddress();
        }
    }
}