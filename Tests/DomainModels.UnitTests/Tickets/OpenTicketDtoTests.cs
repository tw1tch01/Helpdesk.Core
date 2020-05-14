﻿using System;
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
    public class OpenTicketDtoTests
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
            OpenTicketDto dto = null;
            var item = _mapper.Map<Ticket>(dto);
            Assert.IsNull(item);
        }

        [Test]
        public void MapsOpenTicketDtoToTicket()
        {
            var dto = new OpenTicketDto
            {
                Name = _fixture.Create<string>(),
                Description = _fixture.Create<string>(),
                Severity = _fixture.Create<Severity>(),
                Priority = _fixture.Create<Priority>(),
                DueDate = _fixture.Create<DateTimeOffset>(),
                ClientId = _fixture.Create<int>(),
                ProjectId = _fixture.Create<int?>()
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
                Assert.AreEqual(dto.DueDate, ticket.DueDate);
                Assert.AreEqual(dto.ClientId, ticket.ClientId);
                Assert.AreEqual(dto.ProjectId, ticket.ProjectId);
            });
        }
    }
}