using System.Collections.Generic;
using Helpdesk.DomainModels.Common;

namespace Helpdesk.Services.Tickets.Results.Valid
{
    public class TicketUpdatedResult : TicketValidResult
    {
        private const string _message = "Ticket details have been updated.";
        private const string _changesKey = "Changes";

        public TicketUpdatedResult(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
            : base(ticketId, _message)
        {
            Data[_changesKey] = changes;
        }
    }
}