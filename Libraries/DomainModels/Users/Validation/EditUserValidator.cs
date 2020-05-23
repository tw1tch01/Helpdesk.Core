using FluentValidation;

namespace Helpdesk.DomainModels.Users.Validation
{
    public class EditUserValidator : AbstractValidator<EditUser>
    {
        public EditUserValidator()
        {
            RuleFor(r => r.Username).MaximumLength(128);
            RuleFor(r => r.Name).MaximumLength(64);
            RuleFor(r => r.Surname).MaximumLength(64);
            RuleFor(r => r.Alias).MaximumLength(128);
            RuleFor(r => r.Email).EmailAddress().When(c => !string.IsNullOrEmpty(c.Email));
        }
    }
}