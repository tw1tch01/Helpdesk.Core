using System;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.UnassignTicket
{
    public class UnassignTicketResultFactory : IUnassignTicketResultFactory
    {
        public UnassignTicketResult Unassigned(int ticketId, Guid userGuid)
        {
            return new UnassignTicketResult(TicketUnassignResult.Unassigned)
            {
                TicketId = ticketId,
                UnassignedBy = userGuid
            };
        }

        public UnassignTicketResult TicketNotFound(int ticketId)
        {
            return new UnassignTicketResult(TicketUnassignResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }
    }
}