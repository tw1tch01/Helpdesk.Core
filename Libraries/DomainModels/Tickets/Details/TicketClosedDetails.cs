using System;

namespace Helpdesk.DomainModels.Tickets.Details
{
    public class TicketClosedDetails
    {
        public DateTimeOffset ClosedOn { get; set; }
        public int UserId { get; set; }
        public string ClosedBy { get; set; }
    }
}