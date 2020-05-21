using System;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.DeleteTicket
{
    public interface IDeleteTicketResultFactory
    {
        DeleteTicketResult Deleted(int ticketId, Guid userGuid);

        DeleteTicketResult TicketNotFound(int ticketId);

        DeleteTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow);
    }
}