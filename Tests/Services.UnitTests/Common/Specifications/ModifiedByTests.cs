using AutoFixture;
using Helpdesk.Domain.Common;
using Helpdesk.Services.Common.Specifications;
using Moq;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class ModifiedByTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenModifiedByDoesNotMatchValue_ReturnsFalse()
        {
            var modifiedBy = _fixture.Create<int>();
            var mockEntity = new Mock<IModifiedAudit>();
            mockEntity.Setup(a => a.ModifiedBy).Returns(_fixture.Create<int>());
            var specification = new ModifiedBy<IModifiedAudit>(modifiedBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedByMatchesValue_ReturnsTrue()
        {
            var modifiedBy = _fixture.Create<int>();
            var mockEntity = new Mock<IModifiedAudit>();
            mockEntity.Setup(a => a.ModifiedBy).Returns(modifiedBy);
            var specification = new ModifiedBy<IModifiedAudit>(modifiedBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsTrue(satisified);
        }
    }
}