using System;
using System.Collections.Generic;
using Helpdesk.Domain.Common;
using Helpdesk.Domain.Enums;

namespace Helpdesk.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            AssignedUsers = new HashSet<UserTicket>();
            LinkedTickets = new HashSet<TicketLink>();
        }

        public int TicketId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public Severity Severity { get; set; }
        public Priority Priority { get; set; }
        public DateTimeOffset? StartedOn { get; set; }
        public DateTimeOffset? PausedOn { get; set; }
        public DateTimeOffset? ResolvedOn { get; set; }
        public int? ResolvedBy { get; set; }
        public DateTimeOffset? ClosedOn { get; set; }
        public int? ClosedBy { get; set; }
        public DateTimeOffset? ApprovalRequestedOn { get; set; }
        public int? ApprovalUserId { get; set; }
        public DateTimeOffset? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTimeOffset? FeedbackRequestedOn { get; set; }
        public int ClientId { get; set; }
        public int? ProjectId { get; set; }

        #region Navigational Properties

        public virtual ICollection<UserTicket> AssignedUsers { get; private set; }
        public virtual ICollection<TicketLink> LinkedTickets { get; private set; }
        public virtual User ResolvedByUser { get; set; }
        public virtual User ClosedByUser { get; set; }
        public virtual User ApprovalUser { get; set; }
        public virtual User ApprovedByUser { get; set; }
        public virtual Client Client { get; set; }
        public virtual Project Project { get; set; }

        #endregion Navigational Properties

        #region Public Methods

        public virtual TicketStatus GetStatus()
        {
            if (ResolvedOn.HasValue) return TicketStatus.Resolved;

            if (ClosedOn.HasValue) return TicketStatus.Closed;

            if (ApprovedOn.HasValue) return TicketStatus.Approved;

            if (ApprovalRequestedOn.HasValue) return TicketStatus.PendingApproval;

            if (FeedbackRequestedOn.HasValue) return TicketStatus.PendingFeedback;

            if (PausedOn.HasValue) return TicketStatus.OnHold;

            if (StartedOn.HasValue) return TicketStatus.InProgress;

            if (DueDate < DateTimeOffset.UtcNow) return TicketStatus.Overdue;

            return TicketStatus.Open;
        }

        public virtual void Start()
        {
            StartedOn = DateTimeOffset.UtcNow;
        }

        public virtual void Pause()
        {
            PausedOn = DateTimeOffset.UtcNow;
        }

        public virtual void Resolve(int userId)
        {
            ResolvedBy = userId;
            ResolvedOn = DateTimeOffset.UtcNow;
        }

        public virtual void Close(int userId)
        {
            ClosedBy = userId;
            ClosedOn = DateTimeOffset.UtcNow;
        }

        public virtual void Reopen()
        {
            ResolvedOn = null;
            ResolvedBy = null;
            ClosedOn = null;
            ClosedBy = null;
            StartedOn = null;
            PausedOn = null;
            ApprovalRequestedOn = null;
            ApprovalUserId = null;
            ApprovedOn = null;
            ApprovedBy = null;
            FeedbackRequestedOn = null;
        }

        public virtual void RequestApproval(int userId)
        {
            ApprovalUserId = userId;
            ApprovalRequestedOn = DateTimeOffset.UtcNow;
            ApprovedBy = null;
            ApprovedOn = null;
        }

        public virtual void Approve(int userId)
        {
            ApprovedBy = userId;
            ApprovedOn = DateTimeOffset.UtcNow;
            ApprovalUserId = null;
            ApprovalRequestedOn = null;
        }

        public virtual void RequestFeedback()
        {
            FeedbackRequestedOn = DateTimeOffset.UtcNow;
        }

        public virtual void FeedbackReceived()
        {
            FeedbackRequestedOn = null;
        }

        #endregion Public Methods
    }
}