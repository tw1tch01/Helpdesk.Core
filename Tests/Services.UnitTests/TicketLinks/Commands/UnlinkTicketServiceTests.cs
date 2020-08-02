using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.TicketLinks.Commands.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Helpdesk.Services.TicketLinks.Factories.UnlinkTickets;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.TicketLinks.Commands
{
    [TestFixture]
    public class UnlinkTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void Unlink_WhenLinkTicketIsSelfUnlink_ThrowsArgumentException()
        {
            var ticketId = _fixture.Create<int>();
            var unlinkTicket = new UnlinkTicket
            {
                FromTicketId = ticketId,
                ToTicketId = ticketId
            };

            var service = CreateService();

            Assert.ThrowsAsync<ArgumentException>(() => service.Unlink(unlinkTicket), "Should throw an ArgumentException when UnlinkTicket is self unlink.");
        }

        [Test]
        public async Task Link_VerifyThatSingleAsyncForAnOrSpecOfGetLinkedTicketsByIdIsCalled()
        {
            var unlinkTicket = _fixture.Create<UnlinkTicket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.Unlink(unlinkTicket);

            mockContext.Verify(v => v.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>()), Times.Once, "Should call the context's SingleAsync method exactly once for LinqSpecification of TicketLink.");
        }

        [Test]
        public async Task Link_WhenTicketLinkRecordIsNull_VerifyFactoryTicketsNotLinkedIsReturned()
        {
            var unlinkTicket = _fixture.Create<UnlinkTicket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUnlinkTicketsResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>())).ReturnsAsync((TicketLink)null);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Unlink(unlinkTicket);

            mockFactory.Verify(v => v.TicketsNotLinked(unlinkTicket.FromTicketId, unlinkTicket.ToTicketId), Times.Once, "Should return the factory's TicketsNotLinked method.");
        }

        [Test]
        public async Task Link_WhenTicketLinkExists_VerifyRemoveIsCalled()
        {
            var unlinkTicket = _fixture.Create<UnlinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>())).ReturnsAsync(ticketLink);


            var service = CreateService(
                mockContext: mockContext);

            await service.Unlink(unlinkTicket);

            mockContext.Verify(v => v.Remove(ticketLink), Times.Once, "Should call the context's Remove method for the ticket link.");
        }

        [Test]
        public async Task Link_VerifySaveAsyncIsCalled()
        {
            var unlinkTicket = _fixture.Create<UnlinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>())).ReturnsAsync(ticketLink);

            var service = CreateService(
                mockContext: mockContext);

            await service.Unlink(unlinkTicket);

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Link_WhenTicketLinkIsRemoved_VerifyFactoryUnlinkedIsReturned()
        {
            var unlinkTicket = _fixture.Create<UnlinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<IUnlinkTicketsResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>())).ReturnsAsync(ticketLink);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            var result = await service.Unlink(unlinkTicket);

            mockFactory.Verify(v => v.Unlinked(unlinkTicket.FromTicketId, unlinkTicket.ToTicketId), Times.Once, "Should call the factory's Unlinked method.");
        }

        private UnlinkTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<IUnlinkTicketsResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockFactory ??= new Mock<IUnlinkTicketsResultFactory>();

            return new UnlinkTicketService(
                mockContext.Object,
                mockFactory.Object);
        }
    }
}