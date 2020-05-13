using System.Collections.Generic;
using AutoFixture;
using FluentValidation.Results;
using Helpdesk.Services.Extensions;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Extensions
{
    [TestFixture]
    public class ValidationFailureExtensionsTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresIsNull_ReturnsEmptyDictionary()
        {
            IList<ValidationFailure> failures = null;
            var result = failures.GroupPropertyWithErrors();
            Assert.IsEmpty(result, "Should return an empty dictionary.");
        }

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresIsEmpty_ReturnsEmptyDictionary()
        {
            var failures = new List<ValidationFailure>();
            var result = failures.GroupPropertyWithErrors();
            Assert.IsEmpty(result, "Should return an empty dictionary.");
        }

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresHasSinglePropertyError_ReturnsDictionaryWithOnlyOneKeyAndValueContainsOneErrorMessage()
        {
            var propertyName = _fixture.Create<string>();
            var errorMessage = _fixture.Create<string>();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName, errorMessage)
            };
            var result = failures.GroupPropertyWithErrors();
            Assert.Multiple(() =>
            {
                Assert.Contains(propertyName, result.Keys, "Should contain a key for the propertyName.");
                Assert.AreEqual(1, result.Keys.Count, "Should only contian a single entry.");
                Assert.AreEqual(1, result[propertyName].Count, "Should only contain a single entry.");
                Assert.Contains(errorMessage, result[propertyName], "Should contain the errorMessage.");
            });
        }

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresHasDuplicatePropertyErrors_ReturnsDictionaryWithOnlyOneKeyAndValueContainsOneErrorMessage()
        {
            var propertyName = _fixture.Create<string>();
            var errorMessage = _fixture.Create<string>();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName, errorMessage),
                new ValidationFailure(propertyName, errorMessage)
            };
            var result = failures.GroupPropertyWithErrors();
            Assert.Multiple(() =>
            {
                Assert.Contains(propertyName, result.Keys, "Should contain a key for the propertyName.");
                Assert.AreEqual(1, result.Keys.Count, "Should only contian a single entry.");
                Assert.AreEqual(1, result[propertyName].Count, "Should only contain a single entry.");
                Assert.Contains(errorMessage, result[propertyName], "Should contain the errorMessage.");
            });
        }

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresHasSinglePropertyWithMultipleErrors_ReturnsDictionaryWithOnlyOneKeyAndValueContainsAllErrorMessages()
        {
            var propertyName = _fixture.Create<string>();
            var errorMessage1 = _fixture.Create<string>();
            var errorMessage2 = _fixture.Create<string>();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName, errorMessage1),
                new ValidationFailure(propertyName, errorMessage2)
            };
            var result = failures.GroupPropertyWithErrors();
            Assert.Multiple(() =>
            {
                Assert.Contains(propertyName, result.Keys, "Should contain a key for the propertyName.");
                Assert.AreEqual(1, result.Keys.Count, "Should only contian a single entry.");
                Assert.AreEqual(2, result[propertyName].Count, "Should contain 2 entries.");
                Assert.Contains(errorMessage1, result[propertyName], "Should contain errorMessage1.");
                Assert.Contains(errorMessage2, result[propertyName], "Should contain errorMessage2.");
            });
        }

        [Test]
        public void GroupPropertyWithErrors_WhenFailuresHasMultiplePropertyErrors_ReturnsDictionaryWithAllPropertyErrors()
        {
            var propertyName1 = _fixture.Create<string>();
            var propertyName2 = _fixture.Create<string>();
            var errorMessage1 = _fixture.Create<string>();
            var errorMessage2 = _fixture.Create<string>();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(propertyName1, errorMessage1),
                new ValidationFailure(propertyName2, errorMessage2)
            };
            var result = failures.GroupPropertyWithErrors();
            Assert.Multiple(() =>
            {
                Assert.Contains(propertyName1, result.Keys, "Should contain a key for the propertyName1.");
                Assert.Contains(propertyName2, result.Keys, "Should contain a key for the propertyName2.");
                Assert.AreEqual(2, result.Keys.Count, "Should contian 2 entries.");
                Assert.AreEqual(1, result[propertyName1].Count, "Should only contain a single entry.");
                Assert.AreEqual(1, result[propertyName2].Count, "Should only contain a single entry.");
                Assert.Contains(errorMessage1, result[propertyName1], "Should contain errorMessage1.");
                Assert.Contains(errorMessage2, result[propertyName2], "Should contain errorMessage2.");
            });
        }
    }
}