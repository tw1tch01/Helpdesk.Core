using AutoFixture;
using Helpdesk.Services.Tickets.Factories.CloseTicket;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Tickets.Factories
{
    [TestFixture]
    public class CloseTicketResultFactoryTests
    {
        private readonly IFixture _fixture = new Fixture();
        private CloseTicketResultFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new CloseTicketResultFactory();
        }
    }
}