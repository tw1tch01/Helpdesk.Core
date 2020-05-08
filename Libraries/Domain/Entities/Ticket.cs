using System;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
        }

        public int TicketId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? PausedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public int? UserId { get; set; }
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }

        #region Public Methods

        public TicketStatus GetStatus()
        {
            return TicketStatus.Todo;
        }

        #endregion Public Methods
    }
}