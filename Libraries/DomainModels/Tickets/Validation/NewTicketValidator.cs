using FluentValidation;

namespace Helpdesk.DomainModels.Tickets.Validation
{
    public class NewTicketValidator : AbstractValidator<NewTicket>
    {
        public NewTicketValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(64);
            RuleFor(r => r.Description).NotEmpty();
            RuleFor(r => r.Client).NotEmpty();
            RuleFor(r => r.Assignee).NotEmpty().When(r => r.Assignee.HasValue);
        }
    }
}