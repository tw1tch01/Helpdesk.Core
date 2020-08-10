using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.StartTicket;
using Helpdesk.Services.Tickets.Factories.StartTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class StartTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Start_VerifySingleAsyncforGetTicketByIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Start(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Start_WhenTicketRecordINull_ReturnsFactoryTicketNotFound()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Start_WhenTicketIsResolved_ReturnsFactoryTicketAlreadyResolved()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Start_WhenTicketIsClosed_ReturnsFactoryTicketAlreadyClosed()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Start_WhenTicketIsInProgress_ReturnsFactoryTicketAlreadyStarted()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.InProgress);
            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyStarted(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyStarted method.");
        }

        [Test]
        public async Task Start_VerifyTicketStartedIsCalled()
        {
            var userGuid = It.IsAny<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Start(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.Start(userGuid), Times.Once, "Should call the ticket's Start method.");
        }

        [Test]
        public async Task Start_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method exactly once.");
        }

        [Test]
        public async Task Start_WhenTicketIsStarted_VerifyEventIsRaised()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockEventService = new Mock<IEventService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockContext,
                mockEventService: mockEventService);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockEventService.Verify(v => v.Publish(It.IsAny<TicketStartedEvent>()), Times.Once, "Should publish a TicketStartedEvent.");
        }

        [Test]
        public async Task Start_WhenTicketIsStarted_VerifyFactoryStartedIsReturned()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IStartTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Start(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Started(ticket), Times.Once, "Should return the factory's Started method.");
        }

        private StartTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockRepository = null,
            IMock<IStartTicketResultFactory> mockFactory = null,
            IMock<IEventService> mockEventService = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockFactory ??= new Mock<IStartTicketResultFactory>();
            mockEventService ??= new Mock<IEventService>();

            return new StartTicketService(
                mockRepository.Object,
                mockFactory.Object,
                mockEventService.Object);
        }
    }
}