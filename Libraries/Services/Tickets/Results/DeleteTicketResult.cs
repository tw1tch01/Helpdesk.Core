using System;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class DeleteTicketResult : IProcessResult<TicketDeleteResult>
    {
        public DeleteTicketResult(TicketDeleteResult result)
        {
            Result = result;
        }

        public TicketDeleteResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Guid? UserGuid { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketDeleteResult.Deleted => ResultMessages.Deleted,
            TicketDeleteResult.TicketNotFound => ResultMessages.TicketNotFound,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}