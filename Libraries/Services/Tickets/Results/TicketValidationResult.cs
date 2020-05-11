using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Tickets.Results
{
    public abstract class TicketValidationResult : ProcessResult
    {
        private const string _ticketIdKey = nameof(Ticket.TicketId);

        protected TicketValidationResult(bool isValid, string message)
            : base(isValid, message)
        {
        }

        protected TicketValidationResult(int ticketId, bool isValid, string message)
            : base(isValid, message)
        {
            Data[_ticketIdKey] = ticketId;
        }
    }
}