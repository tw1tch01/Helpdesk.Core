using AutoFixture;
using FluentValidation.TestHelper;
using Helpdesk.DomainModels.Tickets.Validation;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Tickets.Validation
{
    [TestFixture]
    public class EditTicketValidatorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private EditTicketValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EditTicketValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldNotHaveErrorWhenNameIsNullOrWhitespace(string name)
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldNotHaveErrorWhenNameIsSpecified()
        {
            var name = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldHaveErrorWhenNameLengthIsGreaterThan64()
        {
            var name = string.Join("", _fixture.CreateMany<string>(3));
            _validator.ShouldHaveValidationErrorFor(t => t.Name, name);
        }
    }
}