using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using NUnit.Framework;

namespace Helpdesk.Domain.UnitTests.Entities
{
    [TestFixture]
    public class TicketTests
    {
        #region GetStatus

        [Test]
        public void GetStatus_WhenResolvedOnHasValue_ReturnsResolved()
        {
            var ticket = new Ticket
            {
                ResolvedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Resolved, status, $"Should return {TicketStatus.Resolved} when {nameof(Ticket.ResolvedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenClosedOnHasValue_ReturnsClosed()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Closed, status, $"Should return {TicketStatus.Closed} when {nameof(Ticket.ClosedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenApprovedOnHasValue_ReturnsApproved()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Approved, status, $"Should return {TicketStatus.Approved} when {nameof(Ticket.ApprovedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenApprovalRequestOnHasValue_ReturnsPendingApproval()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.PendingApproval, status, $"Should return {TicketStatus.PendingApproval} when {nameof(Ticket.ApprovalRequestedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenFeedbackRequestOnHasValue_ReturnsPendingFeedback()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = null,
                FeedbackRequestedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.PendingFeedback, status, $"Should return {TicketStatus.PendingFeedback} when {nameof(Ticket.FeedbackRequestedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenStartedOnHasValue_ReturnsInProgress()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = null,
                FeedbackRequestedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.PendingFeedback, status, $"Should return {TicketStatus.PendingFeedback} when {nameof(Ticket.FeedbackRequestedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenDueDateHasIsLessThanCurrentDate_ReturnsOverdue()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = null,
                FeedbackRequestedOn = null,
                DueDate = DateTime.UtcNow.AddDays(-1)
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Overdue, status, $"Should return {TicketStatus.Overdue} when {nameof(Ticket.DueDate)} is less than current date.");
        }

        [Test]
        public void GetStatus_WhenDueDateIsInFuture_ReturnsOpen()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = null,
                FeedbackRequestedOn = null,
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Open, status, $"Should return {TicketStatus.Open} when {nameof(Ticket.DueDate)} is in the future.");
        }

        [Test]
        public void GetStatus_WhenDueDateIsNull_ReturnsOpen()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                ApprovedOn = null,
                ApprovalRequestedOn = null,
                FeedbackRequestedOn = null,
                DueDate = null
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Open, status, $"Should return {TicketStatus.Open} when {nameof(Ticket.DueDate)} is null.");
        }

        #endregion GetStatus
    }
}