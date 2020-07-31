using System;
using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Tickets;
using Helpdesk.Domain.Tickets.Enums;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Tickets;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets
{
    [TestFixture]
    public class EditTicketTests
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
            EditTicket editTicket = null;
            var item = _mapper.Map<Ticket>(editTicket);
            Assert.IsNull(item);
        }

        [Test]
        public void MapsOpenTicketDtoToTicket()
        {
            var editTicket = new EditTicket
            {
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Severity = _fixture.Create<Severity?>(),
                Priority = _fixture.Create<Priority?>(),
            };
            var ticket = _mapper.Map<Ticket>(editTicket);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket);
                Assert.IsInstanceOf<Ticket>(ticket);
                Assert.AreEqual(editTicket.Name, ticket.Name);
                Assert.AreEqual(editTicket.Description, ticket.Description);
                Assert.AreEqual(editTicket.Severity, ticket.Severity);
                Assert.AreEqual(editTicket.Priority, ticket.Priority);
            });
        }

        [Test]
        public void WhenUpdateDueDateIsTrue_MapsDueDate()
        {
            var editTicket = new EditTicket
            {
                DueDate = _fixture.Create<DateTimeOffset?>(),
                UpdateDueDate = true
            };
            var ticket = _mapper.Map<Ticket>(editTicket);

            Assert.AreEqual(editTicket.DueDate, ticket.DueDate);
        }

        [Test]
        public void WhenUpdateDueDateIsFalse_DoesNotMapDueDate()
        {
            var editTicket = new EditTicket
            {
                DueDate = _fixture.Create<DateTimeOffset>(),
                UpdateDueDate = false
            };
            var ticket = _mapper.Map<Ticket>(editTicket);

            Assert.AreNotEqual(editTicket.DueDate, ticket.DueDate);
        }
    }
}