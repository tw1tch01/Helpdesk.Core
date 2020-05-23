using System;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.UnassignTicket
{
    public interface IUnassignTicketResultFactory
    {
        UnassignTicketResult Unassigned(int ticketId, Guid userGuid);

        UnassignTicketResult TicketNotFound(int ticketId);

        UnassignTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow);
    }
}