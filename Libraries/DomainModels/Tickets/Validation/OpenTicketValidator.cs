using FluentValidation;

namespace Helpdesk.DomainModels.Tickets.Validation
{
    public class OpenTicketValidator : AbstractValidator<OpenTicketDto>
    {
        public OpenTicketValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(64);
            RuleFor(r => r.Description).NotEmpty();
            RuleFor(r => r.ClientId).GreaterThan(0).When(r => r.ClientId.HasValue);
            RuleFor(r => r.ProjectId).GreaterThan(0).When(r => r.ProjectId.HasValue);
        }
    }
}