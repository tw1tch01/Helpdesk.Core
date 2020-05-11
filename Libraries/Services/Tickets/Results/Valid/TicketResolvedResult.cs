using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketResolvedResult : TicketValidResult
    {
        private const string _message = "Ticket has been resolved.";
        private const string _resolvedByKey = nameof(Ticket.ResolvedBy);

        public TicketResolvedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_resolvedByKey] = userId;
        }
    }
}