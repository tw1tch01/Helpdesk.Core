using System;
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
        private readonly Fixture _fixture = new Fixture();

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsSatisfiedBy_WhenValueIsNullOrWhitespace_ThrowsArgumentException(string value)
        {
            Assert.Throws<ArgumentException>(() => new ModifiedBy<IModifiedAudit>(value));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void IsSatisfiedBy_WhenModifiedByIsNull_ReturnsFalse(string value)
        {
            var modifiedBy = _fixture.Create<string>();
            var mockEntity = new Mock<IModifiedAudit>();
            mockEntity.Setup(a => a.ModifiedBy).Returns(value);
            var specification = new ModifiedBy<IModifiedAudit>(modifiedBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedByDoesNotMatchValue_ReturnsFalse()
        {
            var modifiedBy = _fixture.Create<string>();
            var mockEntity = new Mock<IModifiedAudit>();
            mockEntity.Setup(a => a.ModifiedBy).Returns(_fixture.Create<string>());
            var specification = new ModifiedBy<IModifiedAudit>(modifiedBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedByMatchesValue_ReturnsTrue()
        {
            var modifiedBy = _fixture.Create<string>();
            var mockEntity = new Mock<IModifiedAudit>();
            mockEntity.Setup(a => a.ModifiedBy).Returns(modifiedBy);
            var specification = new ModifiedBy<IModifiedAudit>(modifiedBy);
            var satisified = specification.IsSatisfiedBy(mockEntity.Object);
            Assert.IsTrue(satisified);
        }
    }
}