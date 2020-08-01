using System;
using AutoFixture;
using AutoMapper;
using Helpdesk.Domain.Common;
using Helpdesk.DomainModels.Common;
using Helpdesk.DomainModels.Mappings;
using NUnit.Framework;

namespace Helpdesk.DomainModels.UnitTests.Common
{
    [TestFixture]
    public class ModifiedAuditInfoTests
    {
        private readonly IFixture _fixture = new Fixture();
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
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
            TestEntity audit = null;
            var auditInfo = _mapper.Map<ModifiedAuditInfo>(audit);
            Assert.IsNull(auditInfo);
        }

        [Test]
        public void MapsTestEntityToModifiedAuditInfo()
        {
            var audit = _fixture.Create<TestEntity>();
            var auditInfo = _mapper.Map<ModifiedAuditInfo>(audit);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(auditInfo);
                Assert.IsInstanceOf<ModifiedAuditInfo>(auditInfo);
                Assert.AreEqual(audit.ModifiedBy, auditInfo.By);
                Assert.AreEqual(audit.ModifiedOn, auditInfo.On);
                Assert.AreEqual(audit.ModifiedProcess, auditInfo.Process);
            });
        }

        private class TestEntity : IModifiedAudit
        {
            public string ModifiedBy { get; set; }
            public DateTimeOffset? ModifiedOn { get; set; }
            public string ModifiedProcess { get; set; }
        }
    }
}