using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketClosedResult : TicketValidResult
    {
        private const string _message = "Ticket has been closed.";
        private const string _closedByKey = nameof(Ticket.ClosedBy);

        public TicketClosedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_closedByKey] = userId;
        }
    }
}