using System.Threading.Tasks;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.TicketLinks.Commands.LinkTicket
{
    public interface ILinkTicketService
    {
        Task<ProcessResult> Link(int fromTicketId, int toTicketId, TicketLinkType linkType);
    }
}