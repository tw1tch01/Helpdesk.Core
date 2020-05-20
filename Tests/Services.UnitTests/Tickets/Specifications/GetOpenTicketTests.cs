using System;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Specifications
{
    [TestFixture]
    public class GetOpenTicketTests
    {
        [Test]
        public void IsSatisfiedBy_WhenTicketIsOpen_ReturnsTrue()
        {
            var ticket = CreateTicket();
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when status is Open.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIsOverdue_ReturnsTrue()
        {
            var ticket = CreateTicket(dueDate: DateTimeOffset.UtcNow.AddDays(-1));
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when status is Overdue.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIsResolved_ReturnsFalse()
        {
            var ticket = CreateTicket(resolvedOn: DateTimeOffset.UtcNow);
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when status is Resolved.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIsClosed_ReturnsFalse()
        {
            var ticket = CreateTicket(closedOn: DateTimeOffset.UtcNow);
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when status is Closed.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIsInProgress_ReturnsFalse()
        {
            var ticket = CreateTicket(startedOn: DateTimeOffset.UtcNow);
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when status is InProgress.");
        }

        [Test]
        public void IsSatisfiedBy_WhenTicketIsOnHold_ReturnsFalse()
        {
            var ticket = CreateTicket(pausedOn: DateTimeOffset.UtcNow);
            var spec = new GetOpenTickets();
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when status is OnHold.");
        }

        private Ticket CreateTicket(
            DateTimeOffset? dueDate = null,
            DateTimeOffset? startedOn = null,
            DateTimeOffset? pausedOn = null,
            DateTimeOffset? resolvedOn = null,
            DateTimeOffset? closedOn = null)
        {
            return new Ticket()
            {
                DueDate = dueDate,
                StartedOn = startedOn,
                PausedOn = pausedOn,
                ResolvedOn = resolvedOn,
                ClosedOn = closedOn
            };
        }
    }
}