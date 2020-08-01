using System;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

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
    }
}