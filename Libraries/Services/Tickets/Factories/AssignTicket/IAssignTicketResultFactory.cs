using System;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.AssignTicket
{
    public interface IAssignTicketResultFactory
    {
        AssignTicketResult Assigned(Ticket ticket);

        AssignTicketResult TicketAlreadyClosed(Ticket ticket);

        AssignTicketResult TicketAlreadyResolved(Ticket ticket);

        AssignTicketResult TicketNotFound(int ticketId);

        AssignTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow);
    }
}