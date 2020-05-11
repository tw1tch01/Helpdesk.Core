using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketAlreadyResolvedResult : TicketInvalidResult
    {
        private const string _message = "Ticket has already been resolved.";
        private const string _resolvedByKey = nameof(Ticket.ResolvedBy);

        public TicketAlreadyResolvedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_resolvedByKey] = userId;
        }
    }
}