using System;
using Helpdesk.Services.Common.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class ModifiedBeforeTests
    {
        [Test]
        public void IsSatisfiedBy_WhenModifiedBeforeIsNull_ReturnsFalse()
        {
            var modifiedBefore = DateTimeOffset.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = null
            };
            var specification = new ModifiedBefore<TestEntity>(modifiedBefore);
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedBeforeIsAfterValue_ReturnsFalse()
        {
            var modifiedBefore = DateTimeOffset.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = modifiedBefore
            };
            var specification = new ModifiedBefore<TestEntity>(modifiedBefore.AddDays(-1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedBeforeIsBeforeValue_ReturnsTrue()
        {
            var modifiedBefore = DateTimeOffset.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = modifiedBefore
            };
            var specification = new ModifiedBefore<TestEntity>(modifiedBefore.AddDays(1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsTrue(satisified);
        }
    }
}