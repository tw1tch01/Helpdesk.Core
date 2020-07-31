using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Tickets;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets
{
    [TestFixture]
    public class TicketDetailsTests
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
            _fixture.Behaviors.Clear();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            Ticket ticket = null;
            var item = _mapper.Map<TicketDetails>(ticket);
            Assert.IsNull(item);
        }

        [Test]
        public void MapsTicketToTicketDetails()
        {
            var ticket = _fixture.Create<Ticket>();
            var details = _mapper.Map<TicketDetails>(ticket);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(ticket.TicketId, details.TicketId, "Should equal ticket's TicketId.");
                Assert.AreEqual(ticket.Identifier, details.TicketGuid, "Should equal ticket's Identifier.");
                Assert.AreEqual(ticket.CreatedBy, details.Opened.By, "Should equal ticket's CreatedBy.");
                Assert.AreEqual(ticket.CreatedOn, details.Opened.On, "Should equal ticket's CreatedBy.");
                Assert.AreEqual(ticket.CreatedProcess, details.Opened.Process, "Should equal ticket's CreatedProcess.");
                Assert.AreEqual(ticket.ModifiedBy, details.Modified.By, "Should equal ticket's ModifiedBy.");
                Assert.AreEqual(ticket.ModifiedOn, details.Modified.On, "Should equal ticket's ModifiedBy.");
                Assert.AreEqual(ticket.ModifiedProcess, details.Modified.Process, "Should equal ticket's ModifiedProcess.");
                Assert.AreEqual(ticket.UserGuid, details.Client, "Should equal ticket's UserGuid.");
                Assert.AreEqual(ticket.AssignedUserGuid, details.Assignee, "Should equal ticket's UserGuid.");
                Assert.AreEqual(ticket.Name, details.Name, "Should equal ticket's Name.");
                Assert.AreEqual(ticket.Description, details.Description, "Should equal ticket's Description.");
                Assert.AreEqual(ticket.DueDate, details.DueDate, "Should equal ticket's DueDate.");
                Assert.AreEqual(ticket.Severity, details.Severity, "Should equal ticket's Severity.");
                Assert.AreEqual(ticket.Priority, details.Priority, "Should equal ticket's Priority.");
                Assert.AreEqual(ticket.AssignedOn, details.AssignedOn, "Should equal ticket's AssignedOn.");
                Assert.AreEqual(ticket.StartedOn, details.StartedOn, "Should equal ticket's StartedOn.");
                Assert.AreEqual(ticket.StartedBy, details.StartedBy, "Should equal ticket's StartedBy.");
                Assert.AreEqual(ticket.PausedOn, details.PausedOn, "Should equal ticket's PausedOn.");
                Assert.AreEqual(ticket.PausedBy, details.PausedBy, "Should equal ticket's PausedBy.");
                Assert.AreEqual(ticket.ResolvedOn, details.ResolvedOn, "Should equal ticket's ResolvedOn.");
                Assert.AreEqual(ticket.ResolvedBy, details.ResolvedBy, "Should equal ticket's ResolvedBy.");
                Assert.AreEqual(ticket.ClosedOn, details.ClosedOn, "Should equal ticket's ClosedOn.");
                Assert.AreEqual(ticket.ClosedBy, details.ClosedBy, "Should equal ticket's ClosedBy.");
                Assert.AreEqual(ticket.GetStatus(), details.Status, "Should equal ticket's Status.");
            });
        }
    }
}