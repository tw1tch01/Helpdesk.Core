using System;
using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using NUnit.Framework;

namespace Helpdesk.Domain.UnitTests.Entities
{
    [TestFixture]
    public class TicketTests
    {
        private readonly IFixture _fixture = new Fixture();

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

        #endregion GetStatus

        #region Start

        [Test]
        public void Start_WhenTicketIsPaused_SetsPausedOnToNull()
        {
            var ticket = new Ticket
            {
                PausedOn = DateTimeOffset.UtcNow
            };
            ticket.Start();
            Assert.IsNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)} to null.");
        }

        [Test]
        public void Start_WhenStartedOnIsNull_SetsStartedOn()
        {
            var ticket = new Ticket();
            ticket.Start();
            Assert.IsNotNull(ticket.StartedOn, $"Should set {nameof(Ticket.StartedOn)}.");
        }

        [Test]
        public void Start_WhenStartedOnHasValue_DoesNotChangeValue()
        {
            var now = DateTimeOffset.UtcNow;
            var ticket = new Ticket
            {
                StartedOn = now
            };
            ticket.Start();
            Assert.AreEqual(now, ticket.StartedOn, $"Should not change {nameof(Ticket.StartedOn)} when it already has a value.");
        }

        #endregion Start

        #region Pause

        [Test]
        public void Pause_SetsPausedOn()
        {
            var ticket = new Ticket();
            ticket.Pause();
            Assert.IsNotNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)}.");
        }

        #endregion Pause

        #region Resolve

        [Test]
        public void Resolve_SetsResolutionProperties()
        {
            var userId = _fixture.Create<int>();
            var ticket = new Ticket();
            ticket.Resolve(userId);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.ResolvedOn, $"Should set {nameof(Ticket.ResolvedOn)}.");
                Assert.AreEqual(userId, ticket.ResolvedBy, $"Should set {nameof(Ticket.ResolvedBy)} equal to {userId}.");
            });
        }

        #endregion Resolve

        #region Close

        [Test]
        public void Close_SetsClosureProperties()
        {
            var userId = _fixture.Create<int>();
            var ticket = new Ticket();
            ticket.Close(userId);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.ClosedOn, $"Should set {nameof(Ticket.ClosedOn)}.");
                Assert.AreEqual(userId, ticket.ClosedBy, $"Should set {nameof(Ticket.ClosedBy)} equal to {userId}.");
            });
        }

        #endregion Close

        #region Reopen

        [Test]
        public void Reopen_ResetsAllProperties()
        {
            var ticket = new Ticket
            {
                ResolvedOn = _fixture.Create<DateTimeOffset>(),
                ResolvedBy = _fixture.Create<int>(),
                ClosedOn = _fixture.Create<DateTimeOffset>(),
                ClosedBy = _fixture.Create<int>(),
                StartedOn = _fixture.Create<DateTimeOffset>(),
                PausedOn = _fixture.Create<DateTimeOffset>()
            };
            ticket.Reopen();

            Assert.Multiple(() =>
            {
                Assert.IsNull(ticket.ResolvedOn, $"Should set {nameof(Ticket.ResolvedOn)} to null.");
                Assert.IsNull(ticket.ResolvedBy, $"Should set {nameof(Ticket.ResolvedBy)} to null.");
                Assert.IsNull(ticket.ClosedOn, $"Should set {nameof(Ticket.ClosedOn)} to null.");
                Assert.IsNull(ticket.ClosedBy, $"Should set {nameof(Ticket.ClosedBy)} to null.");
                Assert.IsNull(ticket.StartedOn, $"Should set {nameof(Ticket.StartedOn)} to null.");
                Assert.IsNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)} to null.");
            });
        }

        #endregion Reopen
    }
}