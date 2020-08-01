using System;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.UnassignTicket
{
    public interface IUnassignTicketResultFactory
    {
        UnassignTicketResult Unassigned(int ticketId, Guid userGuid);

        UnassignTicketResult TicketNotFound(int ticketId);
    }
}