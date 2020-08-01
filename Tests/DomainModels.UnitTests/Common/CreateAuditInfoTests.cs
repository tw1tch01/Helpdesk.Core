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
    public class CreateAuditInfoTests
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
            var auditInfo = _mapper.Map<CreatedAuditInfo>(audit);
            Assert.IsNull(auditInfo);
        }

        [Test]
        public void MapsTestEntityToCreatedAuditInfo()
        {
            var audit = _fixture.Create<TestEntity>();
            var auditInfo = _mapper.Map<CreatedAuditInfo>(audit);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(auditInfo);
                Assert.IsInstanceOf<CreatedAuditInfo>(auditInfo);
                Assert.AreEqual(audit.CreatedBy, auditInfo.By);
                Assert.AreEqual(audit.CreatedOn, auditInfo.On);
                Assert.AreEqual(audit.CreatedProcess, auditInfo.Process);
            });
        }

        private class TestEntity : ICreatedAudit
        {
            public string CreatedBy { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
            public string CreatedProcess { get; set; }
        }
    }
}