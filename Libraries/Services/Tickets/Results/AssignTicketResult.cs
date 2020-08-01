using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class AssignTicketResult : IProcessResult<TicketAssignResult>
    {
        public AssignTicketResult(TicketAssignResult result)
        {
            Result = result;
        }

        public TicketAssignResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public Guid? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public Guid? ClosedBy { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketAssignResult.Assigned => ResultMessages.Assigned,
            TicketAssignResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketAssignResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketAssignResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}