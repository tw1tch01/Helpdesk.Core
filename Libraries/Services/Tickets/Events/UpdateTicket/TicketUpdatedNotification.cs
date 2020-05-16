using System.Collections.Generic;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Notifications;

namespace Helpdesk.Services.Tickets.Events.UpdateTicket
{
    public class TicketUpdatedNotification : TicketUpdateEvent, INotificationProcess
    {
        public TicketUpdatedNotification(int ticketId, IReadOnlyDictionary<string, ValueChange> changes)
            : base(ticketId, changes)
        {
        }
    }
}