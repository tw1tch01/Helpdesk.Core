using System;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Domain.Tickets
{
    public class TicketLink : ICreatedAudit
    {
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedProcess { get; set; }
        public TicketLinkType LinkType { get; set; }

        #region Navigational Properties

        public Ticket FromTicket { get; set; }
        public Ticket ToTicket { get; set; }

        #endregion Navigational Properties
    }
}