using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.UnassignTicket;
using Helpdesk.Services.Tickets.Factories.UnassignTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class UnassignTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task UnassignUser_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.UnassignUser(ticketId, It.IsAny<Guid>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUnassignTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.UnassignUser(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketCanBeUnassigned_VerifyTicketUnassignMethodIsCalled()
        {
            var userGuid = _fixture.Create<Guid>();
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockContext: mockContext);

            await service.UnassignUser(It.IsAny<int>(), userGuid);

            mockTicket.Verify(v => v.UnassignUser(), Times.Once, "Should call the ticket's Unassign method.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockContext: mockContext);

            await service.UnassignUser(It.IsAny<int>(), It.IsAny<Guid>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task UnassignUser_WhenTicketIsUnassigned_VerifyFactoryUnassignedIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var userGuid = _fixture.Create<Guid>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUnassignTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.UnassignUser(ticketId, userGuid);

            mockFactory.Verify(v => v.Unassigned(ticketId, userGuid), Times.Once, "Should return the factory's Unassigned method.");
        }

        private UnassignTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<IUnassignTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockFactory ??= new Mock<IUnassignTicketResultFactory>();

            return new UnassignTicketService(
                mockContext.Object,
                mockFactory.Object);
        }
    }
}