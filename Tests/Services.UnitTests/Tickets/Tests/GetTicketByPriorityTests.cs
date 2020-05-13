using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetTicketByPriorityTests
    {
        [TestCase(Priority.Urgent)]
        [TestCase(Priority.Major)]
        [TestCase(Priority.Moderate)]
        [TestCase(Priority.Minor)]
        public void IsSatisfiedBy_WhenPriorityMatchesValue_ReturnsTrue(Priority priority)
        {
            var ticket = new Ticket
            {
                Priority = priority
            };
            var spec = new GetTicketByPriority(priority);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when priority matches value.");
        }

        [TestCase(Priority.Urgent, Priority.Major)]
        [TestCase(Priority.Urgent, Priority.Moderate)]
        [TestCase(Priority.Urgent, Priority.Minor)]
        [TestCase(Priority.Major, Priority.Urgent)]
        [TestCase(Priority.Major, Priority.Moderate)]
        [TestCase(Priority.Major, Priority.Minor)]
        [TestCase(Priority.Moderate, Priority.Urgent)]
        [TestCase(Priority.Moderate, Priority.Major)]
        [TestCase(Priority.Moderate, Priority.Minor)]
        [TestCase(Priority.Minor, Priority.Urgent)]
        [TestCase(Priority.Minor, Priority.Major)]
        [TestCase(Priority.Minor, Priority.Moderate)]
        public void IsSatisfiedBy_WhenPriorityDoesNotMatchValue_ReturnsFalse(Priority priority, Priority value)
        {
            var ticket = new Ticket
            {
                Priority = priority
            };
            var spec = new GetTicketByPriority(value);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when priority does not match value.");
        }
    }
}