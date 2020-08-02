using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.TicketLinks;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.TicketLinks
{
    [TestFixture]
    public class UnlinkTicketTests
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
            UnlinkTicket unlinkTicket = null;
            var ticketLink = _mapper.Map<TicketLink>(unlinkTicket);
            Assert.IsNull(ticketLink);
        }

        [Test]
        public void MapsUnlinkTicketsToTicketLink()
        {
            var unlinkTicket = _fixture.Create<LinkTicket>();
            var ticketLink = _mapper.Map<TicketLink>(unlinkTicket);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticketLink);
                Assert.IsInstanceOf<TicketLink>(ticketLink);
                Assert.AreEqual(unlinkTicket.FromTicketId, ticketLink.FromTicketId);
                Assert.AreEqual(unlinkTicket.ToTicketId, ticketLink.ToTicketId);
                Assert.AreEqual(unlinkTicket.LinkType, ticketLink.LinkType);
            });
        }
    }
}