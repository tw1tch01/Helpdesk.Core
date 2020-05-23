using System;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.DeleteTicket
{
    public class DeleteTicketResultFactory : IDeleteTicketResultFactory
    {
        public DeleteTicketResult Deleted(int ticketId, Guid userGuid)
        {
            return new DeleteTicketResult(TicketDeleteResult.Deleted)
            {
                TicketId = ticketId,
                UserGuid = userGuid
            };
        }

        public DeleteTicketResult TicketNotFound(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public DeleteTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow)
        {
            return new DeleteTicketResult(TicketDeleteResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserGuid = userGuid,
                Workflow = workflow
            };
        }
    }
}