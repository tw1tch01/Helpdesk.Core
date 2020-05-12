using System;

namespace Helpdesk.DomainModels.Tickets.Details
{
    public class TicketResolvedDetails
    {
        public DateTimeOffset ResolvedOn { get; set; }
        public int UserId { get; set; }
        public string ResolvedBy { get; set; }
    }
}