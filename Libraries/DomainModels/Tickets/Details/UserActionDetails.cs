using System;

namespace Helpdesk.DomainModels.Tickets.Details
{
    public class UserActionDetails
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTimeOffset On { get; set; }
    }
}