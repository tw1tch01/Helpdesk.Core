using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class ReopenTicketResult : IProcessResult<TicketReopenResult>
    {
        public ReopenTicketResult(TicketReopenResult result)
        {
            Result = result;
        }

        public TicketReopenResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketReopenResult.Reopened => ResultMessages.Reopened,
            TicketReopenResult.TicketNotFound => ResultMessages.TicketNotFound,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}