using System;
using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.ReopenTicket
{
    public interface IReopenTicketService
    {
        Task<ReopenTicketResult> Reopen(int ticketId, Guid userGuid);
    }
}