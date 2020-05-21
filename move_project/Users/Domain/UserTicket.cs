using System;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Entities
{
    public class UserTicket : ICreatedAudit
    {
        public int UserId { get; set; }
        public int TicketId { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedProcess { get; set; }

        #region Navigational Properties

        public virtual User User { get; set; }
        public virtual Ticket Ticket { get; set; }

        #endregion Navigational Properties
    }
}