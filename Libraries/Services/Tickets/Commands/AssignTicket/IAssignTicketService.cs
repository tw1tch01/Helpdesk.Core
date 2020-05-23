using System;
using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.AssignTicket
{
    public interface IAssignTicketService
    {
        Task<AssignTicketResult> AssignUser(int ticketId, Guid userGuid);
    }
}