using Helpdesk.Services.TicketLinks.Results;

namespace Helpdesk.Services.TicketLinks.Factories.UnlinkTickets
{
    public interface IUnlinkTicketsResultFactory
    {
        UnlinkTicketsResult TicketsNotLinked(int fromTicketId, int toTicketId);

        UnlinkTicketsResult Unlinked(int fromTicketId, int toTicketId);
    }
}