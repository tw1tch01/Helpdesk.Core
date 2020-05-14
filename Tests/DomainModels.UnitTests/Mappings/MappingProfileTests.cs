using AutoMapper;
using Helpdesk.DomainModels.Mappings;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Mappings
{
    [TestFixture]
    public class MappingProfileTests
    {
        [Test]
        public void ConfigurationIsValid()
        {
            var configProvider = new MapperConfiguration(opt =>
            {
                opt.AddProfile<MappingProfile>();
            });
            configProvider.AssertConfigurationIsValid();
        }
    }
}