using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Users;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Users
{
    [TestFixture]
    public class UserLookupTests
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
            User user = null;
            var userLookup = _mapper.Map<UserLookup>(user);
            Assert.IsNull(userLookup);
        }

        [Test]
        public void MapsUserToUserLookup()
        {
            var user = _fixture.Create<User>();
            var userLookup = _mapper.Map<UserLookup>(user);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(user.UserId, userLookup.UserId, "Should equal user's UserId.");
                Assert.AreEqual(user.Identifier, userLookup.UserGuid, "Should equal user's Identifier.");
                Assert.AreEqual(user.GetDisplayName(), userLookup.DisplayName, "Should call user's GetDisplayName.");
            });
        }
    }
}