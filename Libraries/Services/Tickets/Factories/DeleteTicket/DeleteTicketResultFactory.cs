using System;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

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
    }
}