using System;
using System.Threading.Tasks;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Commands.PauseTicket
{
    public interface IPauseTicketService
    {
        Task<PauseTicketResult> Pause(int ticketId, Guid userGuid);
    }
}