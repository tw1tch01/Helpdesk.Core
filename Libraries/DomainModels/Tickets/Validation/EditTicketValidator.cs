using FluentValidation;

namespace Helpdesk.DomainModels.Tickets.Validation
{
    public class EditTicketValidator : AbstractValidator<EditTicket>
    {
        public EditTicketValidator()
        {
            RuleFor(r => r.Name).MaximumLength(64);
        }
    }
}