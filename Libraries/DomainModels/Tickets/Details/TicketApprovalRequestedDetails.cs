using System;

namespace Helpdesk.DomainModels.Tickets.Details
{
    public class TicketApprovalRequestedDetails
    {
        public DateTimeOffset ApprovedRequestedOn { get; set; }
        public int UserId { get; set; }
        public string ApprovalRequestedFrom { get; set; }
    }
}