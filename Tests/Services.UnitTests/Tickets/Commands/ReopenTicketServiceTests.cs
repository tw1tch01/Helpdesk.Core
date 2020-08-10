using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.ReopenTicket;
using Helpdesk.Services.Tickets.Factories.ReopenTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class ReopenTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Reopen_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Reopen(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Reopen_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IReopenTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Reopen(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Reopen_VerifyTicketReopenIsCalled()
        {
            var mockTicket = new Mock<Ticket>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Reopen(It.IsAny<int>(), It.IsAny<Guid>());

            mockTicket.Verify(v => v.Reopen(), Times.Once, "Should call the repository's Reopen method.");
        }

        [Test]
        public async Task Reopen_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Reopen(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method.");
        }

        [Test]
        public async Task Reopen_WhenTicketIsReopened_VerifyEventIsRaised()
        {
            var mockContext = new Mock<IEntityRepository<ITicketContext>>();
            var mockEventService = new Mock<IEventService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockContext,
                mockEventService: mockEventService);

            await service.Reopen(It.IsAny<int>(), It.IsAny<Guid>());

            mockEventService.Verify(v => v.Publish(It.IsAny<TicketReopenedEvent>()), Times.Once, "Should publish a TicketReopenedEvent.");
        }

        [Test]
        public async Task Reopen_WhenTicketIsReopened_VerifyFactoryReopenedIsReturned()
        {
            var ticket = new Ticket();
            var userGuid = It.IsAny<Guid>();
            var mockRepository = new Mock<IEntityRepository<ITicketContext>>();
            var mockFactory = new Mock<IReopenTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Reopen(It.IsAny<int>(), userGuid);

            mockFactory.Verify(v => v.Reopened(ticket, userGuid), Times.Once, "Should return the factoy's Reopened method.");
        }

        private ReopenTicketService CreateService(
            IMock<IEntityRepository<ITicketContext>> mockRepository = null,
            IMock<IReopenTicketResultFactory> mockFactory = null,
            IMock<IEventService> mockEventService = null)
        {
            mockRepository ??= new Mock<IEntityRepository<ITicketContext>>();
            mockFactory ??= new Mock<IReopenTicketResultFactory>();
            mockEventService ??= new Mock<IEventService>();

            return new ReopenTicketService(
                mockRepository.Object,
                mockFactory.Object,
                mockEventService.Object);
        }
    }
}