using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.CloseTicket
{
    public class CloseTicketResultFactory : ICloseTicketResultFactory
    {
        public CloseTicketResult Closed(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public CloseTicketResult TicketAlreadyClosed(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public CloseTicketResult TicketAlreadyResolved(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public CloseTicketResult TicketNotFound(int ticketId)
        {
            throw new NotImplementedException();
        }

        public CloseTicketResult UserNotFound(int ticketId, int userId)
        {
            throw new NotImplementedException();
        }

        public CloseTicketResult WorkflowFailed(int ticketId, IWorkflowProcess beforeWorkflow)
        {
            throw new NotImplementedException();
        }
    }
}