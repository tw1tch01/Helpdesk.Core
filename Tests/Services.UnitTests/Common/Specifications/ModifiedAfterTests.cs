using System;
using Helpdesk.Services.Common.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common.Specifications
{
    [TestFixture]
    public class ModifiedAfterTests
    {
        [Test]
        public void IsSatisfiedBy_WhenModifiedAfterIsNull_ReturnsFalse()
        {
            var modifiedAfter = DateTime.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = null
            };
            var specification = new ModifiedAfter<TestEntity>(modifiedAfter.AddDays(1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedAfterIsBeforeValue_ReturnsFalse()
        {
            var modifiedAfter = DateTime.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = modifiedAfter
            };
            var specification = new ModifiedAfter<TestEntity>(modifiedAfter.AddDays(1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsFalse(satisified);
        }

        [Test]
        public void IsSatisfiedBy_WhenModifiedAfterIsAfterValue_ReturnsTrue()
        {
            var modifiedAfter = DateTime.UtcNow;
            var item = new TestEntity
            {
                ModifiedOn = modifiedAfter
            };
            var specification = new ModifiedAfter<TestEntity>(modifiedAfter.AddDays(-1));
            var satisified = specification.IsSatisfiedBy(item);
            Assert.IsTrue(satisified);
        }
    }
}