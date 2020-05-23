using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Services.Common;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Common
{
    [TestFixture]
    public class AbstractLookupTests : AbstractLookup<string>
    {
        public AbstractLookupTests()
            : base(null)
        {
        }

        #region And

        [Test]
        public void And_WhenValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => And(null));
        }

        [Test]
        public void And_WhenSpecificationIsNull_SetsSpecificationEqualToPassedThroughSpec()
        {
            _specification = null;
            var spec = new FakeSpecificatioon();
            And(spec);
            Assert.AreEqual(spec, _specification, "Should equal passed through specification.");
        }

        [Test]
        public void And_WhenSpecificationIsNotNull_ShouldBeAssignableFromAnAndLinqSpecification()
        {
            _specification = new FakeSpecificatioon();
            var spec = new FakeSpecificatioon();
            And(spec);
            Assert.IsAssignableFrom<AndLinqSpecification<string>>(_specification, "Should equal composition of passed through specification and _specification.");
        }

        #endregion

        #region AndNot

        [Test]
        public void AndNot_WhenValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => AndNot(null));
        }

        [Test]
        public void AndNot_WhenSpecificationIsNull_ShouldBeAssignableFromNotLinqSpecification()
        {
            _specification = null;
            var spec = new FakeSpecificatioon();
            AndNot(spec);
            Assert.IsAssignableFrom<NotLinqSpecification<string>>(_specification, "Should equal the not composition of the specification.");
        }

        [Test]
        public void AndNot_WhenSpecificationIsNotNull_ShouldBeAssignableFromAnAndLinqSpecification()
        {
            _specification = new FakeSpecificatioon();
            var spec = new FakeSpecificatioon();
            AndNot(spec);
            Assert.IsAssignableFrom<AndLinqSpecification<string>>(_specification, "Should equal and composition of passed through specification and _specification.");
        }

        #endregion

        #region Or

        [Test]
        public void Or_WhenValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Or(null));
        }

        [Test]
        public void Or_WhenSpecificationIsNull_SetsSpecificationEqualToPassedThroughSpec()
        {
            _specification = null;
            var spec = new FakeSpecificatioon();
            Or(spec);
            Assert.AreEqual(spec, _specification, "Should equal passed through specification.");
        }

        [Test]
        public void Or_WhenSpecificationIsNotNull_ShouldBeAssignableFromAnOrLinqSpecification()
        {
            _specification = new FakeSpecificatioon();
            var spec = new FakeSpecificatioon();
            Or(spec);
            Assert.IsAssignableFrom<OrLinqSpecification<string>>(_specification, "Should equal or composition of passed through specification and _specification.");
        }

        #endregion

        #region OrNot

        [Test]
        public void OrNot_WhenValueIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => OrNot(null));
        }

        [Test]
        public void OrNot_WhenSpecificationIsNull_ShouldBeAssignableFromNotLinqSpecification()
        {
            _specification = null;
            var spec = new FakeSpecificatioon();
            OrNot(spec);
            Assert.IsAssignableFrom<NotLinqSpecification<string>>(_specification, "Should equal passed through specification.");
        }

        [Test]
        public void OrNot_WhenSpecificationIsNotNull_ShouldBeAssignableFromAnOrLinqSpecification()
        {
            _specification = new FakeSpecificatioon();
            var spec = new FakeSpecificatioon();
            OrNot(spec);
            Assert.IsAssignableFrom<OrLinqSpecification<string>>(_specification, "Should equal composition of passed through specification and _specification.");
        }

        #endregion


        #region ValidatePaging

        [Test]
        public void ValidatePaging_WhenPageIsLessThanZero_SetsPageToZero()
        {
            int page = -1;

            var result = ValidatePaging(page, 10);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(0, result.page);
                Assert.AreNotEqual(page, result.page);
            });
        }

        [TestCase(0)]
        [TestCase(11)]
        public void ValidatePaging_WhenPageIsGreaterThanOrEqualToZero_LeavesPageUnchanged(int page)
        {
            var result = ValidatePaging(page, 10);

            Assert.AreEqual(page, result.page);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ValidatePaging_WhenPageSizeIsLessThanOrEqualToZero_SetsPageSizeEqualToDefaultPageSize(int pageSize)
        {
            var result = ValidatePaging(0, pageSize);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_defaultPageSize, result.pageSize);
                Assert.AreNotEqual(pageSize, result.pageSize);
            });
        }

        [Test]
        public void ValidatePaging_WhenPageSizeIsGreaterThanMaximumPageSize_SetsPageSizeEqualToMaximumPageSize()
        {
            var pageSize = _maximumPageSize + 1;

            var result = ValidatePaging(0, pageSize);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(_maximumPageSize, result.pageSize);
                Assert.AreNotEqual(pageSize, result.pageSize);
            });
        }

        [Test]
        public void ValidatePaging_WhenPageSizeIsBetweenDefaultAndMaximumPageSize_LeavesPageSizeUnchanged()
        {
            var pageSize = 50;

            var result = ValidatePaging(0, pageSize);

            Assert.AreEqual(pageSize, result.pageSize);
        }

        #endregion ValidatePaging

        private class FakeSpecificatioon : LinqSpecification<string>
        {
            public override Expression<Func<string, bool>> AsExpression()
            {
                return s => true;
            }
        }
    }
}