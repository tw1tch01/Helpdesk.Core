using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketApprovedResult : TicketValidResult
    {
        private const string _message = "Ticket has been approved.";
        private const string _approvedByKey = nameof(Ticket.ApprovedBy);

        public TicketApprovedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_approvedByKey] = userId;
        }
    }
}