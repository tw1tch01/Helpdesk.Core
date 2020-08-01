using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Tickets;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets
{
    [TestFixture]
    public class NewTicketTests
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
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            NewTicket newTicket = null;
            var ticket = _mapper.Map<Ticket>(newTicket);
            Assert.IsNull(ticket);
        }

        [Test]
        public void MapsNewTicketToTicket()
        {
            var newTicket = _fixture.Create<NewTicket>();
            var ticket = _mapper.Map<Ticket>(newTicket);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket);
                Assert.IsInstanceOf<Ticket>(ticket);
                Assert.AreEqual(newTicket.Client, ticket.UserGuid);
                Assert.AreEqual(newTicket.Assignee, ticket.AssignedUserGuid);
                Assert.AreEqual(newTicket.Name, ticket.Name);
                Assert.AreEqual(newTicket.Description, ticket.Description);
                Assert.AreEqual(newTicket.DueDate, ticket.DueDate);
                Assert.AreEqual(newTicket.Severity, ticket.Severity);
                Assert.AreEqual(newTicket.Priority, ticket.Priority);
            });
        }
    }
}