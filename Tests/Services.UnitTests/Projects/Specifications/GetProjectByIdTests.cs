using AutoFixture;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Projects.Specifications;
using NUnit.Framework;

namespace Helpdesk.Services.UnitTests.Projects.Specifications
{
    [TestFixture]
    public class GetProjectByIdTests
    {
        private readonly IFixture _fixture = new Fixture();

        [Test]
        public void IsSatisfiedBy_WhenProjectIdMatchesValue_ReturnsTrue()
        {
            var projectId = _fixture.Create<int>();
            var project = new Project
            {
                ProjectId = projectId
            };
            var spec = new GetProjectById(projectId);
            var result = spec.IsSatisfiedBy(project);
            Assert.IsTrue(result, "Should return true when Project's projectId matches paramter value.");
        }

        [Test]
        public void IsSatisfiedBy_WhenProjectIdDoesNotMatchValue_ReturnsFalse()
        {
            var projectId = _fixture.Create<int>();
            var project = new Project
            {
                ProjectId = _fixture.Create<int>()
            };
            var spec = new GetProjectById(projectId);
            var result = spec.IsSatisfiedBy(project);
            Assert.IsFalse(result, "Should return false when Project's projectId does not match paramter value.");
        }
    }
}