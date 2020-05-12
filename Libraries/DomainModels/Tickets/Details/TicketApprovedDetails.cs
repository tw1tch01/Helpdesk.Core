using System;

namespace Helpdesk.DomainModels.Tickets.Details
{
    public class TicketApprovedDetails
    {
        public DateTimeOffset ApprovedOn { get; set; }
        public int UserId { get; set; }
        public string ApprovedBy { get; set; }
    }
}