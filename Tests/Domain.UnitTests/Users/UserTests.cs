using AutoFixture;
using Helpdesk.Domain.Users;
using NUnit.Framework;

namespace Helpdesk.Domain.UnitTests.Users
{
    [TestFixture]
    public class UserTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void GetDisplayName_WhenAliasIsNotNullOrEmpty_ReturnsAlias()
        {
            var alias = _fixture.Create<string>();
            var user = new User
            {
                Alias = alias
            };
            var result = user.GetDisplayName();
            Assert.AreEqual(alias, result, "Should return Alias when it is not null or empty");
        }

        [Test]
        public void GetDisplayName_WhenAliasIsNullOrEmpty_ReturnsSubstringOfNameAndSurname()
        {
            var name = _fixture.Create<string>();
            var surname = _fixture.Create<string>();
            var user = new User
            {
                Name = name,
                Surname = surname
            };
            var result = user.GetDisplayName();
            Assert.AreEqual(name + " " + surname, result, "Should return substring of Name and Surname.");
        }
    }
}