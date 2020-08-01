using System;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.DeleteTicket
{
    public interface IDeleteTicketResultFactory
    {
        DeleteTicketResult Deleted(int ticketId, Guid userGuid);

        DeleteTicketResult TicketNotFound(int ticketId);
    }
}