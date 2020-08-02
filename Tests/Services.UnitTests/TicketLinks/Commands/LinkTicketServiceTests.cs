using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Data.Repositories;
using Data.Specifications;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.TicketLinks;
using Helpdesk.Services.Common.Contexts;
using Helpdesk.Services.TicketLinks.Commands.LinkTickets;
using Helpdesk.Services.TicketLinks.Factories.LinkTickets;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.TicketLinks.Commands
{
    [TestFixture]
    public class LinkTicketServiceTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void Link_WhenLinkTicketIsSelfLink_ThrowsArgumentException()
        {
            var ticketId = _fixture.Create<int>();
            var newLink = new LinkTicket
            {
                FromTicketId = ticketId,
                ToTicketId = ticketId
            };

            var service = CreateService();

            Assert.ThrowsAsync<ArgumentException>(() => service.Link(newLink), "Should throw an ArgumentException when LinkTicket is self link.");
        }

        [Test]
        public async Task Link_VerifyThatSingleAsyncForAnOrSpecOfGetLinkedTicketsByIdIsCalled()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();

            var service = CreateService(mockContext: mockContext);

            await service.Link(linkTicket);

            mockContext.Verify(v => v.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>()), Times.Once, "Should call the context's SingleAsync method exactly once for LinqSpecification of TicketLink.");
        }

        [Test]
        public async Task Link_WhenTicketLinkRecordIsNotNull_VerifyFactoryTicketsAlreadyLinkedIsReturned()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockFactory = new Mock<ILinkTicketsResultFactory>();

            mockContext.Setup(s => s.SingleAsync(It.IsAny<LinqSpecification<TicketLink>>())).ReturnsAsync(ticketLink);

            var service = CreateService(
                mockContext: mockContext,
                mockFactory: mockFactory);

            await service.Link(linkTicket);

            mockFactory.Verify(v => v.TicketsAlreadyLinked(ticketLink), Times.Once, "Should return the factory's TicketsAlreadyLinked method.");
        }

        [Test]
        public async Task Link_VerifyAddAsyncIsCalled()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<TicketLink>(linkTicket)).Returns(ticketLink);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper);

            await service.Link(linkTicket);

            mockContext.Verify(v => v.AddAsync(ticketLink), Times.Once, "Should call the context's AddAsync method for the ticket link.");
        }

        [Test]
        public async Task Link_VerifySaveAsyncIsCalled()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<TicketLink>(linkTicket)).Returns(ticketLink);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper);

            await service.Link(linkTicket);

            mockContext.Verify(v => v.SaveAsync(), Times.Once, "Should call the context's SaveAsync.");
        }

        [Test]
        public async Task Link_WhenTicketLinkIsAdded_VerifyFactoryLinkedIsReturned()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = new TicketLink();
            var mockContext = new Mock<IContextRepository<ITicketContext>>();
            var mockMapper = new Mock<IMapper>();
            var mockFactory = new Mock<ILinkTicketsResultFactory>();

            mockMapper.Setup(m => m.Map<TicketLink>(linkTicket)).Returns(ticketLink);

            var service = CreateService(
                mockContext: mockContext,
                mockMapper: mockMapper,
                mockFactory: mockFactory);

            var result = await service.Link(linkTicket);

            mockFactory.Verify(v => v.Linked(ticketLink), Times.Once, "Should call the factory's Linked method.");
        }

        private LinkTicketService CreateService(
            IMock<IContextRepository<ITicketContext>> mockContext = null,
            IMock<IMapper> mockMapper = null,
            IMock<ILinkTicketsResultFactory> mockFactory = null)
        {
            mockContext ??= new Mock<IContextRepository<ITicketContext>>();
            mockMapper ??= new Mock<IMapper>();
            mockFactory ??= new Mock<ILinkTicketsResultFactory>();

            return new LinkTicketService(
                mockContext.Object,
                mockMapper.Object,
                mockFactory.Object);
        }
    }
}