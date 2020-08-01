using System.Net.Mail;
using AutoFixture;
using FluentValidation.TestHelper;
using Helpdesk.DomainModels.Users.Validation;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Users.Validators
{
    [TestFixture]
    public class NewUserValidatorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private NewUserValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new NewUserValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        public void ShouldHaveError_WhenNameIsNullOrEmpty(string name)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldNotHaveError_WhenNameIsSpecified()
        {
            var name = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Name, name);
        }

        [Test]
        public void ShouldHaveError_WhenNameLengthIsGreaterThan64()
        {
            var name = string.Join("", _fixture.CreateMany<string>(3));
            _validator.ShouldHaveValidationErrorFor(t => t.Name, name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShoulHaveError_WhenUsernameIsNullOrWhitespace(string username)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Username, username);
        }

        [Test]
        public void ShouldNotHaveError_WhenUsernameIsSpecified()
        {
            var username = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Username, username);
        }

        [Test]
        public void ShouldHaveError_WhenUsernameLengthIsGreaterThan128()
        {
            var username = string.Join("", _fixture.CreateMany<string>(6));
            _validator.ShouldHaveValidationErrorFor(t => t.Username, username);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldNotHaveError_WhenSurnameIsNullOrEmpty(string surname)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Surname, surname);
        }

        [Test]
        public void ShouldNotHaveError_WhenSurnameIsSpecified()
        {
            var surname = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Surname, surname);
        }

        [Test]
        public void ShouldHaveError_WhenSurnameLengthIsGreaterThan64()
        {
            var surname = string.Join("", _fixture.CreateMany<string>(3));
            _validator.ShouldHaveValidationErrorFor(t => t.Surname, surname);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldNotHaveError_WhenAliasIsNullOrWhitespace(string alias)
        {
            _validator.ShouldNotHaveValidationErrorFor(t => t.Alias, alias);
        }

        [Test]
        public void ShouldNotHaveError_WhenAliasIsSpecified()
        {
            var alias = _fixture.Create<string>();
            _validator.ShouldNotHaveValidationErrorFor(t => t.Alias, alias);
        }

        [Test]
        public void ShouldHaveError_WhenAliasLengthIsGreaterThan128()
        {
            var alias = string.Join("", _fixture.CreateMany<string>(6));
            _validator.ShouldHaveValidationErrorFor(t => t.Alias, alias);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void ShouldHaveError_WhenEmailIsNullOrWhitespace(string email)
        {
            _validator.ShouldHaveValidationErrorFor(t => t.Email, email);
        }

        [Test]
        public void ShouldNotHaveError_WhenEmailIsSpecified()
        {
            var email = _fixture.Create<MailAddress>().Address;
            _validator.ShouldNotHaveValidationErrorFor(t => t.Email, email);
        }

        [Test]
        public void ShouldHaveError_WhenEmailIsInvalid()
        {
            var email = _fixture.Create<string>();
            _validator.ShouldHaveValidationErrorFor(t => t.Email, email);
        }
    }
}