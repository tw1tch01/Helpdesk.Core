using System;
using Helpdesk.Services.Common.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class CreatedBeforeTests
    {
        [Test]
        public void IsSatisfiedBy_WhenCreatedBeforeIsAfterValue_ReturnsFalse()
        {
            var createdBefore = DateTime.UtcNow;
            var item = new TestEntity
            {
                CreatedOn = createdBefore
            };
            var specification = new CreatedBefore<TestEntity>(createdBefore.AddDays(-1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenCreatedBeforeIsBeforeValue_ReturnsTrue()
        {
            var createdBefore = DateTime.UtcNow;
            var item = new TestEntity
            {
                CreatedOn = createdBefore
            };
            var specification = new CreatedBefore<TestEntity>(createdBefore.AddDays(1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsTrue(satisified);
        }
    }
}