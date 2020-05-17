using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.DeleteTicket
{
    public class DeleteTicketResultFactory : IDeleteTicketResultFactory
    {
        public DeleteTicketResult Deleted(int ticketId, int userId)
        {
            return new DeleteTicketResult(TicketDeleteResult.Deleted)
            {
                TicketId = ticketId,
                UserId = userId
            };
        }

        public DeleteTicketResult TicketNotFound(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        public DeleteTicketResult WorkflowFailed(int ticketId, int userId, IWorkflowProcess workflow)
        {
            return new DeleteTicketResult(TicketDeleteResult.WorkflowFailed)
            {
                TicketId = ticketId,
                UserId = userId,
                Workflow = workflow
            };
        }
    }
}