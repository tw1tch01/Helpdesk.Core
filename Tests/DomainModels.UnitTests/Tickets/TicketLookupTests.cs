using System;
using System.Collections.Generic;
using System.Text;
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
            _fixture.Behaviors.Clear();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            Ticket ticket = null;
            var item = _mapper.Map<TicketLookup>(ticket);
            Assert.IsNull(item);
        }
    }
}
