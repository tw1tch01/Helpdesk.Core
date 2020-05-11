using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.UserTickets.Results
{
    public abstract class UserTicketValidationResult : ProcessResult
    {
        private const string _ticketIdKey = nameof(UserTicket.TicketId);
        private const string _userIdKey = nameof(UserTicket.UserId);

        protected UserTicketValidationResult(bool isValid, string message)
            : base(isValid, message)
        {
        }

        protected UserTicketValidationResult(int ticketId, int userId, bool isValid, string message)
            : base(isValid, message)
        {
            Data[_ticketIdKey] = ticketId;
            Data[_userIdKey] = userId;
        }
    }
}