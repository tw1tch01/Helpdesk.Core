using System;
using System.Collections.Generic;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Tickets.Enums;

namespace Helpdesk.Domain.Tickets
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            LinkedTo = new HashSet<TicketLink>();
            LinkedFrom = new HashSet<TicketLink>();
        }

        public int TicketId { get; set; }
        public Guid UserGuid { get; set; }
        public Guid? AssignedUserGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public DateTimeOffset? AssignedOn { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public Guid? StartedBy { get; set; }
        public DateTimeOffset? PausedOn { get; set; }
        public Guid? PausedBy { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public Guid? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public Guid? ClosedBy { get; set; }

        #region Navigational Properties

        public ICollection<TicketLink> LinkedTo { get; set; }
        public ICollection<TicketLink> LinkedFrom { get; set; }

        #endregion Navigational Properties

        #region Public Methods

        public virtual TicketStatus GetStatus()
        {
            if (ResolvedOn.HasValue) return TicketStatus.Resolved;

            if (ClosedOn.HasValue) return TicketStatus.Closed;

            if (PausedOn.HasValue) return TicketStatus.OnHold;

            if (StartedOn.HasValue) return TicketStatus.InProgress;

            if (DueDate < DateTimeOffset.UtcNow) return TicketStatus.Overdue;

            return TicketStatus.Open;
        }

        public virtual void Start(Guid userGuid)
        {
            if (!StartedOn.HasValue)
            {
                StartedBy = userGuid;
                StartedOn = DateTimeOffset.UtcNow;
            }
            PausedOn = null;
            PausedBy = null;
        }

        public virtual void Pause(Guid userGuid)
        {
            PausedOn = DateTimeOffset.UtcNow;
            PausedBy = userGuid;
        }

        public virtual void Resolve(Guid userGuid)
        {
            ResolvedOn = DateTimeOffset.UtcNow;
            ResolvedBy = userGuid;
        }

        public virtual void Close(Guid userGuid)
        {
            ClosedOn = DateTimeOffset.UtcNow;
            ClosedBy = userGuid;
        }

        public virtual void Reopen()
        {
            ResolvedOn = null;
            ResolvedBy = null;
            ClosedOn = null;
            ClosedBy = null;
            PausedOn = null;
            PausedBy = null;
            StartedOn = null;
            StartedBy = null;
        }

        public virtual void AssignUser(Guid userGuid)
        {
            AssignedUserGuid = userGuid;
            AssignedOn = DateTimeOffset.UtcNow;
        }

        public virtual void UnassignUser()
        {
            AssignedUserGuid = null;
            AssignedOn = null;
        }

        #endregion Public Methods
    }
}