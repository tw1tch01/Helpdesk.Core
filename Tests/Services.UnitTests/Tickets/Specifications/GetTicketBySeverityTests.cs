using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Tickets.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Tests
{
    [TestFixture]
    public class GetTicketBySeverityTests
    {
        [TestCase(Severity.Low)]
        [TestCase(Severity.Medium)]
        [TestCase(Severity.High)]
        [TestCase(Severity.Critical)]
        public void IsSatisfiedBy_WhenSeverityMatchesValue_ReturnsTrue(Severity severity)
        {
            var ticket = new Ticket
            {
                Severity = severity
            };
            var spec = new GetTicketBySeverity(severity);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsTrue(result, "Should return true when severity matches value.");
        }

        [TestCase(Severity.Low, Severity.Medium)]
        [TestCase(Severity.Low, Severity.High)]
        [TestCase(Severity.Low, Severity.Critical)]
        [TestCase(Severity.Medium, Severity.Low)]
        [TestCase(Severity.Medium, Severity.High)]
        [TestCase(Severity.Medium, Severity.Critical)]
        [TestCase(Severity.High, Severity.Low)]
        [TestCase(Severity.High, Severity.Medium)]
        [TestCase(Severity.High, Severity.Critical)]
        [TestCase(Severity.Critical, Severity.Low)]
        [TestCase(Severity.Critical, Severity.Medium)]
        [TestCase(Severity.Critical, Severity.High)]
        public void IsSatisfiedBy_WhenSeverityDoesNotMatchValue_ReturnsFalse(Severity severity, Severity value)
        {
            var ticket = new Ticket
            {
                Severity = severity
            };
            var spec = new GetTicketBySeverity(value);
            var result = spec.IsSatisfiedBy(ticket);
            Assert.IsFalse(result, "Should return false when severity does not match value.");
        }
    }
}