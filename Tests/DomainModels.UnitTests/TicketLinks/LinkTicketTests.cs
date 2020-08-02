using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.TicketLinks;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.TicketLinks
{
    [TestFixture]
    public class LinkTicketTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var configProvider = new MapperConfiguration(opt =>
            {
                opt.AddProfile<MappingProfile>();
            });
            _mapper = configProvider.CreateMapper();
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            LinkTicket linkTicket = null;
            var ticketLink = _mapper.Map<TicketLink>(linkTicket);
            Assert.IsNull(ticketLink);
        }

        [Test]
        public void MapsLinkTicketToTicketLink()
        {
            var linkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = _mapper.Map<TicketLink>(linkTicket);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticketLink);
                Assert.IsInstanceOf<TicketLink>(ticketLink);
                Assert.AreEqual(linkTicket.FromTicketId, ticketLink.FromTicketId);
                Assert.AreEqual(linkTicket.ToTicketId, ticketLink.ToTicketId);
                Assert.AreEqual(linkTicket.LinkType, ticketLink.LinkType);
            });
        }
    }
}