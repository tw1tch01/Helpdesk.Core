using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Tickets;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets
{
    [TestFixture]
    public class TicketLookupTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var configProvider = new MapperConfiguration(opt =>
            {
                opt.AddProfile<MappingProfile>();
            });
            _mapper = configProvider.CreateMapper();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            Ticket ticket = null;
            var lookup = _mapper.Map<TicketLookup>(ticket);
            Assert.IsNull(lookup);
        }

        [Test]
        public void MapsTicketToTicketLookup()
        {
            var ticket = _fixture.Create<Ticket>();
            var lookup = _mapper.Map<TicketLookup>(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(ticket.TicketId, lookup.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.Identifier, lookup.TicketGuid, "Should equal ticket's Identifier.");
                Assert.AreEqual(ticket.UserGuid, lookup.Client, "Should equal ticket's UserGuid.");
                Assert.AreEqual(ticket.AssignedUserGuid, lookup.Assignee, "Should equal ticket's UserGuid.");
                Assert.AreEqual(ticket.Name, lookup.Name, "Should equal ticket's Name.");
                Assert.AreEqual(ticket.DueDate, lookup.DueDate, "Should equal ticket's DueDate.");
                Assert.AreEqual(ticket.Severity, lookup.Severity, "Should equal ticket's Severity.");
                Assert.AreEqual(ticket.Priority, lookup.Priority, "Should equal ticket's Priority.");
                Assert.AreEqual(ticket.GetStatus(), lookup.Status, "Should equal ticket's Status.");
            });
        }
    }
}