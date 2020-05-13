using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetTicketByStatusTests
    {
        [Test]
        public void IsSatisfiedBy_WhenFilteringForOpenTicketsAndTicketIsOpen_ReturnsTrue()
        {
            var ticket = CreateTicket();
            var spec = new GetTicketByStatus(TicketStatus.Open);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when status matches value.");
        }

        [Test]
        public void IsSatisfiedBy_WhenFilteringForInProgressTicketsAndTicketIsStarted_ReturnsTrue()
        {
            var ticket = CreateTicket(startedOn: DateTimeOffset.UtcNow);
            var spec = new GetTicketByStatus(TicketStatus.InProgress);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when status matches value.");
        }

        private Ticket CreateTicket(
            DateTimeOffset? dueDate = null,
            DateTimeOffset? startedOn = null,
            DateTimeOffset? pausedOn = null,
            DateTimeOffset? resolvedOn = null,
            DateTimeOffset? closedOn = null,
            DateTimeOffset? approvalRequestedOn = null,
            DateTimeOffset? approvedOn = null,
            DateTimeOffset? feedbackRequestedOn = null)
        {
            return new Ticket()
            {
                DueDate = dueDate,
                StartedOn = startedOn,
                PausedOn = pausedOn,
                ResolvedOn = resolvedOn,
                ClosedOn = closedOn,
                ApprovalRequestedOn = approvalRequestedOn,
                ApprovedOn = approvedOn,
                FeedbackRequestedOn = feedbackRequestedOn
            };
        }
    }
}