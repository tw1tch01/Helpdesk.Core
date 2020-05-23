using System;
using AutoFixture;
using FluentValidation.TestHelper;
using Helpdesk.DomainModels.Tickets.Validation;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets.Validation
{
    [TestFixture]
    public class NewTicketValidatorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private NewTicketValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new NewTicketValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldHaveErrorWhenNameIsNullOrWhitespace(string name)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldHaveErrorWhenNameLengthIsGreaterThan64()
        {
            var name = string.Join("", _fixture.CreateMany<string>(3));
            _validator.ShouldHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldNotHaveErrorWhenNameIsSpecified()
        {
            var name = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Name, name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldHaveErrorWhenDescriptionIsNullOrWhitespace(string description)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Description, description);
        }

        [Test]
        public void ShouldNotHaveErrorWhenDescriptionIsSpecified()
        {
            var name = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldHaveErrorClientIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Client, Guid.Empty);
        }

        [Test]
        public void ShouldNotHaveErrorWhenClientHasGeneratedGuid()
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.Client, _fixture.Create<Guid>());
        }

        [Test]
        public void ShouldNotHaveErrorWhenAssigneeIsNull()
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.Assignee, null as Guid?);
        }

        public void ShouldHaveErrorWhenAssigneeIsEmpty()
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Assignee, Guid.Empty);
        }

        [Test]
        public void ShouldNotHaveErrorWhenAssigneeHasGeneratedGuid()
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.Assignee, _fixture.Create<Guid>());
        }
    }
}