using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.UpdateTicket
{
    public class UpdateTicketResultFactory : IUpdateTicketResultFactory
    {
        public UpdateTicketResult TicketNotFound(int ticketId)
        {
            return new UpdateTicketResult(TicketUpdateResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public UpdateTicketResult Updated(Ticket ticket, IReadOnlyDictionary<string, ValueChange> changes)
        {
            return new UpdateTicketResult(TicketUpdateResult.Updated)
            {
                TicketId = ticket.TicketId,
                PropertyChanges = changes
            };
        }

        public UpdateTicketResult ValidationFailure(int ticketId, IList<ValidationFailure> errors)
        {
            return new UpdateTicketResult(TicketUpdateResult.ValidationFailure)
            {
                TicketId = ticketId,
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }
    }
}