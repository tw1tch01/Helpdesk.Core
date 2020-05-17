using AutoFixture;
using Helpdesk.Domain.Common;
using Helpdesk.Services.Common.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class CreatedByTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenCreatedByDoesNotMatchValue_ReturnsFalse()
        {
            var createdBy = _fixture.Create<int>();
            var mockEntity = new Mock<ICreatedAudit>();
            mockEntity.Setup(a => a.CreatedBy).Returns(_fixture.Create<int>());
            var specification = new CreatedBy<ICreatedAudit>(createdBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenCreatedByMatchesValue_ReturnsTrue()
        {
            var createdBy = _fixture.Create<int>();
            var mockEntity = new Mock<ICreatedAudit>();
            mockEntity.Setup(a => a.CreatedBy).Returns(createdBy);
            var specification = new CreatedBy<ICreatedAudit>(createdBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsTrue(satisified);
        }
    }
}