using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Users;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Users
{
    [TestFixture]
    public class NewUserTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var configProvider = new MapperConfiguration(opt =>
            {
                opt.AddProfile<MappingProfile>();
            });
            _mapper = configProvider.CreateMapper();
        }

        [Test]
        public void NullObjectReturnsNull()
        {
            NewUser newUser = null;
            var user = _mapper.Map<User>(newUser);
            Assert.IsNull(user);
        }

        [Test]
        public void MapsNewUserToUser()
        {
            var newUser = _fixture.Create<NewUser>();
            var user = _mapper.Map<User>(newUser);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(newUser.Username, user.Username, "Should equal user's Username.");
                Assert.AreEqual(newUser.Name, user.Name, "Should equal user's Name.");
                Assert.AreEqual(newUser.Surname, user.Surname, "Should equal user's Surname.");
                Assert.AreEqual(newUser.Alias, user.Alias, "Should equal user's Alias.");
                Assert.AreEqual(newUser.Email, user.Email, "Should equal user's Email.");
            });
        }
    }
}