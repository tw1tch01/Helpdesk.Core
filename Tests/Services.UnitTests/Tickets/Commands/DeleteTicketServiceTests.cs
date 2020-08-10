using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Events;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.DeleteTicket;
using Helpdesk.Services.Tickets.Factories.DeleteTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class DeleteTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Delete_VerifySingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockRepository: mockRepository);

            await service.Delete(ticketId, It.IsAny<Guid>());

            mockRepository.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the repository's SingleAsync exactly once for GetTicketById.");
        }

        [Test]
        public async Task Delete_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IDeleteTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Delete(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Delete_VerifyRemoveForTicketIsCalled()
        {
            var ticket = new Ticket();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Delete(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.Remove(ticket), Times.Once, "Should call the repository's Remove method.");
        }

        [Test]
        public async Task Delete_VerifySaveAsyncIsCalled()
        {
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockRepository);

            await service.Delete(It.IsAny<int>(), It.IsAny<Guid>());

            mockRepository.Verify(v => v.SaveAsync(), Times.Once, "Should call the repository's SaveAsync method.");
        }

        [Test]
        public async Task Delete_WhenTicketIsDeleted_VerifyEventIsRaised()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockEventService = new Mock<IEventService>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockContext,
                mockEventService: mockEventService);

            await service.Delete(It.IsAny<int>(), It.IsAny<Guid>());

            mockEventService.Verify(v => v.Publish(It.IsAny<TicketDeletedEvent>()), Times.Once, "Should publish a TicketDeletedEvent.");
        }

        [Test]
        public async Task Delete_WhenTicketIsDeleted_VerifyFactoryDeletedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockRepository = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IDeleteTicketResultFactory>();

            mockRepository.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockRepository: mockRepository,
                mockFactory: mockFactory);

            await service.Delete(ticketId, userGuid);

            mockFactory.Verify(v => v.Deleted(ticketId, userGuid), Times.Once, "Should return the factoy's Deleted method.");
        }

        private DeleteTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockRepository = null,
            IMock<IDeleteTicketResultFactory> mockFactory = null,
            IMock<IEventService> mockEventService = null)
        {
            mockRepository ??= new Mock<IContextRepository<ITicketContext>>();
            mockFactory ??= new Mock<IDeleteTicketResultFactory>();
            mockEventService ??= new Mock<IEventService>();

            return new DeleteTicketService(
                mockRepository.Object,
                mockFactory.Object,
                mockEventService.Object);
        }
    }
}