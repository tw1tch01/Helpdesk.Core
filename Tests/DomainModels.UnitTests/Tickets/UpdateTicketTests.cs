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
    public class UpdateTicketTests
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
            EditTicket dto = null;
            var item = _mapper.Map<Ticket>(dto);
            Assert.IsNull(item);
        }

        [Test]
        public void MapsOpenTicketDtoToTicket()
        {
            var dto = new EditTicket
            {
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Severity = _fixture.Create<Severity?>(),
                Priority = _fixture.Create<Priority?>(),
            };
            var ticket = _mapper.Map<Ticket>(dto);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket);
                Assert.IsInstanceOf<Ticket>(ticket);
                Assert.AreEqual(dto.Name, ticket.Name);
                Assert.AreEqual(dto.Description, ticket.Description);
                Assert.AreEqual(dto.Severity, ticket.Severity);
                Assert.AreEqual(dto.Priority, ticket.Priority);
            });
        }

        [Test]
        public void WhenUpdateDueDateIsTrue_MapsDueDate()
        {
            var dto = new EditTicket
            {
                DueDate = _fixture.Create<DateTimeOffset?>(),
                UpdateDueDate = true
            };
            var ticket = _mapper.Map<Ticket>(dto);

            Assert.AreEqual(dto.DueDate, ticket.DueDate);
        }

        [Test]
        public void WhenUpdateDueDateIsFalse_DoesNotMapDueDate()
        {
            var dto = new EditTicket
            {
                DueDate = _fixture.Create<DateTimeOffset>(),
                UpdateDueDate = false
            };
            var ticket = _mapper.Map<Ticket>(dto);

            Assert.AreNotEqual(dto.DueDate, ticket.DueDate);
        }
    }
}