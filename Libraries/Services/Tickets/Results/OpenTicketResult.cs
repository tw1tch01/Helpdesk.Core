using System.Collections.Generic;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class OpenTicketResult : IProcessResult<TicketOpenResult>, IValidationResult
    {
        public OpenTicketResult(TicketOpenResult result)
        {
            Result = result;
        }

        public TicketOpenResult Result { get; }

        public string Message => GetMessage();

        public int? TicketId { get; set; }

        public int? ClientId { get; set; }

        public int? ProjectId { get; set; }

        public Dictionary<string, List<string>> ValidationFailures { get; internal set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketOpenResult.Opened => ResultMessages.Opened,
            TicketOpenResult.ValidationFailure => ResultMessages.ValidationFailure,
            TicketOpenResult.ClientNotFound => ResultMessages.ClientNotFound,
            TicketOpenResult.ProjectNotFound => ResultMessages.ProjectNotFound,
            TicketOpenResult.ProjectInaccessible => ResultMessages.ProjectInaccessible,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}