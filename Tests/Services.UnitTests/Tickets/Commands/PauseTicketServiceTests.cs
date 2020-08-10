using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.PauseTicket;
using Helpdesk.Services.Tickets.Factories.PauseTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class PauseTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Pause_VerifySingleAsyncforGetTicketByIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Pause(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Pause_WhenTicketRecordINull_ReturnsFactoryTicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsResolved_ReturnsFactoryTicketAlreadyResolved()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsOnHold_ReturnsFactoryTicketAlreadyPaused()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.OnHold);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyPaused(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyPaused method.");
        }

        [Test]
        public async Task Pause_WhenTicketIsClosed_ReturnsFactoryTicketAlreadyClosed()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Pause_VerifyTicketPausedIsCalled()
        {
            var userGuid = It.IsAny<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Pause(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.Pause(userGuid), Times.Once, "Should call the ticket's Pause method.");
        }

        [Test]
        public async Task Pause_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method exactly once.");
        }

        [Test]
        public async Task Pause_WhenTicketIsPaused_VerifyEventIsRaised()
        {
            var mockContext = new Mock<IEntityRepository<ITicketContext>>();
            var mockEventService = new Mock<IEventService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockContext,
                mockEventService: mockEventService);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockEventService.Verify(v => v.Publish(It.IsAny<TicketPausedEvent>()), Times.Once, "Should publish a TicketPausedEvent.");
        }

        [Test]
        public async Task Pause_WhenTicketIsPaused_VerifyFactoryPausedIsReturned()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IPauseTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Pause(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Paused(ticket), Times.Once, "Should return the factory's Paused method.");
        }

        private PauseTicketService CreateService(
            IMock<IEntityRepository<ITicketContext>> mockRepository = null,
            IMock<IPauseTicketResultFactory> mockFactory = null,
            IMock<IEventService> mockEventService = null)
        {
            mockRepository ??= new Mock<IEntityRepository<ITicketContext>>();
            mockFactory ??= new Mock<IPauseTicketResultFactory>();
            mockEventService ??= new Mock<IEventService>();

            return new PauseTicketService(
                mockRepository.Object,
                mockFactory.Object,
                mockEventService.Object);
        }
    }
}