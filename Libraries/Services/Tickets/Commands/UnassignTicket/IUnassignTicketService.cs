using System;
using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.UnassignTicket
{
    public interface IUnassignTicketService
    {
        Task<UnassignTicketResult> UnassignUser(int ticketId, Guid userGuid);
    }
}