using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketAlreadyClosedResult : TicketInvalidResult
    {
        private const string _message = "Ticket has already been closed.";
        private const string _closedByKey = nameof(Ticket.ClosedBy);

        public TicketAlreadyClosedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_closedByKey] = userId;
        }
    }
}