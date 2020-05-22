using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.UpdateTicket
{
    public interface IUpdateTicketResultFactory
    {
        UpdateTicketResult TicketNotFound(int ticketId);

        UpdateTicketResult Updated(Ticket ticket, IReadOnlyDictionary<string, ValueChange> changes);

        UpdateTicketResult ValidationFailure(int ticketId, IList<ValidationFailure> errors);

        UpdateTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow);
    }
}