using FluentValidation;

namespace Helpdesk.DomainModels.Tickets.Validation
{
    public class UpdateTicketValidator : AbstractValidator<UpdateTicket>
    {
        public UpdateTicketValidator()
        {
            RuleFor(r => r.Name).MaximumLength(64);
        }
    }
}