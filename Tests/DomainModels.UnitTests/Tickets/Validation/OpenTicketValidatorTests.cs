using AutoFixture;
using FluentValidation.TestHelper;
using Helpdesk.DomainModels.Tickets.Validation;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets.Validation
{
    [TestFixture]
    public class OpenTicketValidatorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private OpenTicketValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new OpenTicketValidator();
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

        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldHaveErrorWhenClientIdIsNotGreaterThan0(int clientId)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.ClientId, clientId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenClientIdIsGreaterThan0()
        {
            var clientId = _fixture.Create<int>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.ClientId, clientId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenProjectIdIsNull()
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.ProjectId, null as int?);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldHaveErrorWhenProjectIdIsNotGreatherThan0(int projectId)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.ProjectId, projectId);
        }

        [Test]
        public void ShouldNotHaveErrorWhenProjectIdIsGreaterThan0()
        {
            var projectId = _fixture.Create<int>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.ProjectId, projectId);
        }
    }
}