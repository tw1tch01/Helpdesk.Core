using System;
using AutoFixture;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
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

        [Test]
        public void GetStatus_WhenPausedOnHasValue_ReturnsOnHold()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                PausedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.OnHold, status, $"Should return {TicketStatus.OnHold} when {nameof(Ticket.PausedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenStartedOnOnHasValue_ReturnsInProgress()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                PausedOn = null,
                StartedOn = DateTime.UtcNow
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.InProgress, status, $"Should return {TicketStatus.InProgress} when {nameof(Ticket.StartedOn)} has value.");
        }

        [Test]
        public void GetStatus_WhenDueDateIsLessThanCurrentDate_ReturnsOverdue()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                PausedOn = null,
                StartedOn = null,
                DueDate = DateTimeOffset.UtcNow.AddDays(-1)
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Overdue, status, $"Should return {TicketStatus.Overdue} when {nameof(Ticket.DueDate)} has value.");
        }

        [Test]
        public void GetStatus_WhenDueDateIsInTheFuture_ReturnsOpen()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                PausedOn = null,
                StartedOn = null,
                DueDate = DateTimeOffset.UtcNow.AddDays(1)
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Open, status, $"Should return {TicketStatus.Open} when no action has been taken.");
        }

        [Test]
        public void GetStatus_WhenNoActionHasBeenTaken_ReturnsOpen()
        {
            var ticket = new Ticket
            {
                ResolvedOn = null,
                ClosedOn = null,
                PausedOn = null,
                StartedOn = null,
                DueDate = null
            };

            var status = ticket.GetStatus();
            Assert.AreEqual(TicketStatus.Open, status, $"Should return {TicketStatus.Open} when no action has been taken.");
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
            ticket.Start(_fixture.Create<Guid>());
            Assert.IsNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)} to null.");
        }

        [Test]
        public void Start_WhenStartedOnIsNull_SetsStartedOn()
        {
            var ticket = new Ticket();
            ticket.Start(_fixture.Create<Guid>());
            Assert.IsNotNull(ticket.StartedOn, $"Should set {nameof(Ticket.StartedOn)}.");
        }

        [Test]
        public void Start_SetsStartedOn()
        {
            var userGuid = _fixture.Create<Guid>();
            var ticket = new Ticket();
            ticket.Start(userGuid);
            Assert.AreEqual(userGuid, ticket.StartedBy, $"Should set {nameof(Ticket.StartedBy)}.");
        }

        [Test]
        public void Start_WhenStartedOnHasValue_DoesNotChangeValue()
        {
            var now = DateTimeOffset.UtcNow;
            var ticket = new Ticket
            {
                StartedOn = now
            };
            ticket.Start(_fixture.Create<Guid>());
            Assert.AreEqual(now, ticket.StartedOn, $"Should not change {nameof(Ticket.StartedOn)} when it already has a value.");
        }

        #endregion Start

        #region Pause

        [Test]
        public void Pause_SetsPausedOn()
        {
            var userGuid = _fixture.Create<Guid>();
            var ticket = new Ticket();
            ticket.Pause(userGuid);
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)}.");
                Assert.AreEqual(userGuid, ticket.PausedBy, $"Should equal passed through userGuid.");
            });
        }

        #endregion Pause

        #region Resolve

        [Test]
        public void Resolve_SetsResolutionProperties()
        {
            var userGuid = _fixture.Create<Guid>();
            var ticket = new Ticket();
            ticket.Resolve(userGuid);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.ResolvedOn, $"Should set {nameof(Ticket.ResolvedOn)}.");
                Assert.AreEqual(userGuid, ticket.ResolvedBy, $"Should set {nameof(Ticket.ResolvedBy)} equal to {userGuid}.");
            });
        }

        #endregion Resolve

        #region Close

        [Test]
        public void Close_SetsClosureProperties()
        {
            var userGuid = _fixture.Create<Guid>();
            var ticket = new Ticket();
            ticket.Close(userGuid);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.ClosedOn, $"Should set {nameof(Ticket.ClosedOn)}.");
                Assert.AreEqual(userGuid, ticket.ClosedBy, $"Should set {nameof(Ticket.ClosedBy)} equal to {userGuid}.");
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
                ResolvedBy = _fixture.Create<Guid>(),
                ClosedOn = _fixture.Create<DateTimeOffset>(),
                ClosedBy = _fixture.Create<Guid>(),
                PausedOn = _fixture.Create<DateTimeOffset>(),
                PausedBy = _fixture.Create<Guid>(),
                StartedOn = _fixture.Create<DateTimeOffset>(),
                StartedBy = _fixture.Create<Guid>()
            };
            ticket.Reopen();

            Assert.Multiple(() =>
            {
                Assert.IsNull(ticket.ResolvedOn, $"Should set {nameof(Ticket.ResolvedOn)} to null.");
                Assert.IsNull(ticket.ResolvedBy, $"Should set {nameof(Ticket.ResolvedBy)} to null.");
                Assert.IsNull(ticket.ClosedOn, $"Should set {nameof(Ticket.ClosedOn)} to null.");
                Assert.IsNull(ticket.ClosedBy, $"Should set {nameof(Ticket.ClosedBy)} to null.");
                Assert.IsNull(ticket.PausedOn, $"Should set {nameof(Ticket.PausedOn)} to null.");
                Assert.IsNull(ticket.PausedBy, $"Should set {nameof(Ticket.PausedBy)} to null.");
                Assert.IsNull(ticket.StartedOn, $"Should set {nameof(Ticket.StartedOn)} to null.");
                Assert.IsNull(ticket.StartedBy, $"Should set {nameof(Ticket.StartedBy)} to null.");
            });
        }

        #endregion Reopen

        #region AssignUser

        [Test]
        public void AssignUser_SetsAssignmentProperties()
        {
            var userGuid = _fixture.Create<Guid>();
            var ticket = new Ticket
            {
                AssignedOn = null,
                AssignedUserGuid = null
            };

            ticket.AssignUser(userGuid);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket.AssignedOn, $"Should set {nameof(Ticket.AssignedOn)}.");
                Assert.AreEqual(userGuid, ticket.AssignedUserGuid, "Should equal the passed through userGuid.");
            });
        }

        #endregion AssignUser

        #region UnassignUser

        [Test]
        public void UnassignUser_UnsetsAssignmentProperties()
        {
            var ticket = new Ticket
            {
                AssignedOn = _fixture.Create<DateTimeOffset>(),
                AssignedUserGuid = _fixture.Create<Guid>()
            };

            ticket.UnassignUser();

            Assert.Multiple(() =>
            {
                Assert.IsNull(ticket.AssignedOn, $"Should set {nameof(Ticket.AssignedOn)} to null.");
                Assert.IsNull(ticket.AssignedUserGuid, $"Should set {nameof(Ticket.AssignedUserGuid)} to null.");
            });
        }

        #endregion UnassignUser
    }
}