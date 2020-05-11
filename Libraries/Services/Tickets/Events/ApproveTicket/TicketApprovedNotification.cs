using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.ApproveTicket
{
    public class TicketApprovedNotification : TicketApprovedEvent, INotificationProcess
    {
        public TicketApprovedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}