using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Users;
using Helpdesk.DomainModels.Mappings;
using Helpdesk.DomainModels.Users;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Users
{
    [TestFixture]
    public class EditUserTests
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
            EditUser editUser = null;
            var user = _mapper.Map<User>(editUser);
            Assert.IsNull(user);
        }

        [Test]
        public void MapsEditUserToUser()
        {
            var editUser = _fixture.Create<EditUser>();
            var user = _mapper.Map<User>(editUser);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(editUser.Username, user.Username, "Should equal user's Username.");
                Assert.AreEqual(editUser.Name, user.Name, "Should equal user's Name.");
                Assert.AreEqual(editUser.Surname, user.Surname, "Should equal user's Surname.");
                Assert.AreEqual(editUser.Alias, user.Alias, "Should equal user's Alias.");
                Assert.AreEqual(editUser.Email, user.Email, "Should equal user's Email.");
            });
        }
    }
}