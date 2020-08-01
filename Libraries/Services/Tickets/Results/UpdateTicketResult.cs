using System.Collections.Generic;
using Helpdesk.Domain.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class UpdateTicketResult : IProcessResult<TicketUpdateResult>, IValidationResult, IUpdateResult
    {
        public UpdateTicketResult(TicketUpdateResult result)
        {
            Result = result;
        }

        public TicketUpdateResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public Dictionary<string, List<string>> ValidationFailures { get; set; }
        public IReadOnlyDictionary<string, ValueChange> PropertyChanges { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketUpdateResult.Updated => ResultMessages.Updated,
            TicketUpdateResult.ValidationFailure => ResultMessages.ValidationFailure,
            TicketUpdateResult.TicketNotFound => ResultMessages.TicketNotFound,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}