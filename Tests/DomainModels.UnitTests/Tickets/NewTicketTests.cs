using System;
using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
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
            var item = _mapper.Map<Ticket>(newTicket);
            Assert.IsNull(item);
        }

        [Test]
        public void MapsOpenTicketDtoToTicket()
        {
            var newTicket = new NewTicket
            {
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Severity = _fixture.Create<Severity>(),
                Priority = _fixture.Create<Priority>(),
                DueDate = _fixture.Create<DateTimeOffset?>(),
                ClientId = _fixture.Create<int>(),
                ProjectId = _fixture.Create<int?>()
            };
            var ticket = _mapper.Map<Ticket>(newTicket);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(ticket);
                Assert.IsInstanceOf<Ticket>(ticket);
                Assert.AreEqual(newTicket.Name, ticket.Name);
                Assert.AreEqual(newTicket.Description, ticket.Description);
                Assert.AreEqual(newTicket.Severity, ticket.Severity);
                Assert.AreEqual(newTicket.Priority, ticket.Priority);
                Assert.AreEqual(newTicket.DueDate, ticket.DueDate);
                Assert.AreEqual(newTicket.ClientId, ticket.ClientId);
                Assert.AreEqual(newTicket.ProjectId, ticket.ProjectId);
            });
        }
    }
}