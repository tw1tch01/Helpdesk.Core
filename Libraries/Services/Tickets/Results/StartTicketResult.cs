using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class StartTicketResult : IProcessResult<TicketStartResult>
    {
        public StartTicketResult(TicketStartResult result)
        {
            Result = result;
        }

        public TicketStartResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public Guid? StartedBy { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public Guid? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public Guid? ClosedBy { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketStartResult.Started => ResultMessages.Started,
            TicketStartResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketStartResult.TicketAlreadyResolved => ResultMessages.TicketAlreadyResolved,
            TicketStartResult.TicketAlreadyClosed => ResultMessages.TicketAlreadyClosed,
            TicketStartResult.TicketAlreadyStarted => ResultMessages.TicketAlreadyStarted,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}