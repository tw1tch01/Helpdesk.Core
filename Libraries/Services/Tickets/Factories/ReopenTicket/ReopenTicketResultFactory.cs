using System;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.ReopenTicket
{
    public class ReopenTicketResultFactory : IReopenTicketResultFactory
    {
        public ReopenTicketResult Reopened(Ticket ticket, Guid userGuid)
        {
            return new ReopenTicketResult(TicketReopenResult.Reopened)
            {
                TicketId = ticket.TicketId,
                UserGuid = userGuid
            };
        }

        public ReopenTicketResult TicketNotFound(int ticketId)
        {
            return new ReopenTicketResult(TicketReopenResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public ReopenTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow)
        {
            return new ReopenTicketResult(TicketReopenResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserGuid = userGuid,
                Workflow = workflow
            };
        }
    }
}