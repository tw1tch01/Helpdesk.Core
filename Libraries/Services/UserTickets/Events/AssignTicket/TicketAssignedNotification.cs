using Helpdesk.DomainModels.UserTickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.UserTickets.Events.AssignTicket
{
    public class TicketAssignedNotification : TicketAssignedEvent, INotificationProcess
    {
        public TicketAssignedNotification(int ticketId, int userId)
            : base(ticketId, userId)
        {
        }
    }
}