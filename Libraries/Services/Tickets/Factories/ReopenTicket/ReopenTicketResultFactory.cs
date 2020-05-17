using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.ReopenTicket
{
    public class ReopenTicketResultFactory : IReopenTicketResultFactory
    {
        public ReopenTicketResult Reopened(Ticket ticket)
        {
            return new ReopenTicketResult(TicketReopenResult.Reopened)
            {
                TicketId = ticket.TicketId,
                //UserId = userId
            };
        }

        public ReopenTicketResult TicketNotFound(int ticketId)
        {
            return new ReopenTicketResult(TicketReopenResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public ReopenTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow)
        {
            return new ReopenTicketResult(TicketReopenResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserId = userId,
                Workflow = workflow
            };
        }
    }
}