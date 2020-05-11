using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.RequestApproval
{
    public class TicketApprovalRequestNotification : TicketApprovalRequestEvent, INotificationProcess
    {
        public TicketApprovalRequestNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}