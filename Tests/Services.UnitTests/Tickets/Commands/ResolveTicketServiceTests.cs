using System;
using System.Threading.Tasks;
using AutoFixture;
using Data.Repositories;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.Tickets.Commands.ResolveTicket;
using Helpdesk.Services.Tickets.Factories.ResolveTicket;
using Helpdesk.Services.Tickets.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Commands
{
    [TestFixture]
    public class ResolveTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public async Task Resolve_VerifyThatSingleAsyncForGetTicketByIdIsCalled()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.Resolve(ticketId, It.IsAny<Guid>());

            mockContext.Verify(v => v.SingleAsync(It.Is<GetTicketById>(t => t.TicketId == ticketId)), Times.Once, "Should call the context's SingleAsync method exactly once for GetTicketById.");
        }

        [Test]
        public async Task Resolve_WhenTicketRecordIsNull_VerifyFactoryTicketNotFoundIsReturned()
        {
            var ticketId = _fixture.Create<int>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync((Ticket)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(ticketId, It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketNotFound(ticketId), Times.Once, "Should return the factory's TicketNotFound method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyFactoryTicketAlreadyResolvedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Resolved);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyResolved(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyResolved method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsClosed_VerifyFactoryTicketAlreadyClosedIsReturned()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);
            mockTicket.Setup(s => s.GetStatus()).Returns(TicketStatus.Closed);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.TicketAlreadyClosed(mockTicket.Object), Times.Once, "Should return the factory's TicketAlreadyClosed method.");
        }

        [Test]
        public async Task Resolve_WhenTicketCanBeResolved_VerifyTicketResolveMethodIsCalled()
        {
            var mockTicket = new Mock<Ticket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(mockTicket.Object);

            var service = CreateService(
                mockContext: mockContext);

            await service.Resolve(It.IsAny<int>(), It.IsAny<Guid>());

            mockTicket.Verify(v => v.Resolve(It.IsAny<Guid>()), Times.Once, "Should call the ticket's Resolve method.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifySaveAsyncIsCalled()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(new Ticket());

            var service = CreateService(
                mockContext: mockContext);

            await service.Resolve(It.IsAny<int>(), It.IsAny<Guid>());

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Resolve_WhenTicketIsResolved_VerifyFactoryResolvedIsReturned()
        {
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IResolveTicketResultFactory>();

            var ticket = new Ticket();
            mockContext.Setup(s => s.SingleAsync(It.IsAny<GetTicketById>())).ReturnsAsync(ticket);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Resolve(It.IsAny<int>(), It.IsAny<Guid>());

            mockFactory.Verify(v => v.Resolved(ticket), Times.Once, "Should return the factory's Resolved method.");
        }

        private ResolveTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<IResolveTicketResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockFactory ??= new Mock<IResolveTicketResultFactory>();

            return new ResolveTicketService(
                mockContext.Object,
                mockFactory.Object);
        }
    }
}