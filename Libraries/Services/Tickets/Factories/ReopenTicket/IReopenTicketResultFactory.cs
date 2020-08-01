using System;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.ReopenTicket
{
    public interface IReopenTicketResultFactory
    {
        ReopenTicketResult Reopened(Ticket ticket, Guid userGuid);

        ReopenTicketResult TicketNotFound(int ticketId);
    }
}