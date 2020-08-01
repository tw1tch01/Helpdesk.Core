using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Users;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Users
{
    [TestFixture]
    public class UserDetailsTests
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
            var userDetails = _mapper.Map<UserDetails>(user);
            Assert.IsNull(userDetails);
        }

        [Test]
        public void MapsUserToUserDetails()
        {
            var user = _fixture.Create<User>();
            var userDetails = _mapper.Map<UserDetails>(user);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(user.UserId, userDetails.UserId, "Should equal user's UserID.");
                Assert.AreEqual(user.Identifier, userDetails.UserGuid, "Should equal user's Identifier.");
                Assert.AreEqual(user.CreatedBy, userDetails.Created.By, "Should equal user's CreatedBy.");
                Assert.AreEqual(user.CreatedOn, userDetails.Created.On, "Should equal user's CreatedOn.");
                Assert.AreEqual(user.CreatedProcess, userDetails.Created.Process, "Should equal user's CreatedProcess.");
                Assert.AreEqual(user.ModifiedBy, userDetails.Modified.By, "Should equal user's ModifiedBy.");
                Assert.AreEqual(user.ModifiedOn, userDetails.Modified.On, "Should equal user's ModifiedOn.");
                Assert.AreEqual(user.ModifiedProcess, userDetails.Modified.Process, "Should equal user's ModifiedProcess.");
                Assert.AreEqual(user.Username, userDetails.Username, "Should equal user's Username.");
                Assert.AreEqual(user.Name, userDetails.Name, "Should equal user's Name.");
                Assert.AreEqual(user.Surname, userDetails.Surname, "Should equal user's Surname.");
                Assert.AreEqual(user.Alias, userDetails.Alias, "Should equal user's Alias.");
                Assert.AreEqual(user.Email, userDetails.Email, "Should equal user's Email.");
            });
        }
    }
}