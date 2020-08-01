using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class UnassignTicketResult : IProcessResult<TicketUnassignResult>
    {
        public UnassignTicketResult(TicketUnassignResult result)
        {
            Result = result;
        }

        public TicketUnassignResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UnassignedBy { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketUnassignResult.Unassigned => ResultMessages.Unassigned,
            TicketUnassignResult.TicketNotFound => ResultMessages.TicketNotFound,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}