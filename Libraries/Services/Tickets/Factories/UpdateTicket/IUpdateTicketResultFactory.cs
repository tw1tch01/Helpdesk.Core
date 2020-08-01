using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.UpdateTicket
{
    public interface IUpdateTicketResultFactory
    {
        UpdateTicketResult TicketNotFound(int ticketId);

        UpdateTicketResult Updated(Ticket ticket, IReadOnlyDictionary<string, ValueChange> changes);

        UpdateTicketResult ValidationFailure(int ticketId, IList<ValidationFailure> errors);
    }
}