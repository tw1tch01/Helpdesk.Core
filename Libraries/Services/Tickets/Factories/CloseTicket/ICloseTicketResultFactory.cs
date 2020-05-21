using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Factories.CloseTicket
{
    public interface ICloseTicketResultFactory
    {
        CloseTicketResult Closed(Ticket ticket);

        CloseTicketResult TicketAlreadyClosed(Ticket ticket);

        CloseTicketResult TicketAlreadyResolved(Ticket ticket);

        CloseTicketResult TicketNotFound(int ticketId);

        CloseTicketResult UserNotFound(int ticketId, Guid userGuid);

        CloseTicketResult WorkflowFailed(int ticketId, Guid userGuid, IWorkflowProcess workflow);
    }
}