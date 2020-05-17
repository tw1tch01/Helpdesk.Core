using System;
using Helpdesk.Services.Common.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class CreatedAfterTests
    {
        [Test]
        public void IsSatisfiedBy_WhenCreatedAfterIsBeforeValue_ReturnsFalse()
        {
            var createdAfter = DateTime.UtcNow;
            var item = new TestEntity
            {
                CreatedOn = createdAfter
            };
            var specification = new CreatedAfter<TestEntity>(createdAfter.AddDays(1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenCreatedAfterIsAfterValue_ReturnsTrue()
        {
            var createdAfter = DateTime.UtcNow;
            var item = new TestEntity
            {
                CreatedOn = createdAfter
            };
            var specification = new CreatedAfter<TestEntity>(createdAfter.AddDays(-1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsTrue(satisified);
        }
    }
}