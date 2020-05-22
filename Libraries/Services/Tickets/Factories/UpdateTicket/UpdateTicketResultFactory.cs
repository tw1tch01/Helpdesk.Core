using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

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

        public UpdateTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new UpdateTicketResult(TicketUpdateResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }
    }
}