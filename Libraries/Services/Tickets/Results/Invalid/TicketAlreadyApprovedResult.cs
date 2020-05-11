using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Tickets.Results.Invalid
{
    public class TicketAlreadyApprovedResult : TicketInvalidResult
    {
        private const string _message = "Ticket has already been approved.";
        private const string _approvedByKey = nameof(Ticket.ApprovedBy);

        public TicketAlreadyApprovedResult(int ticketId, int userId)
            : base(ticketId, _message)
        {
            Data[_approvedByKey] = userId;
        }
    }
}