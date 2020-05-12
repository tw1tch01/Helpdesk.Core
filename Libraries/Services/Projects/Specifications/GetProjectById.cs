using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Projects.Specifications
{
    public class GetProjectById : LinqSpecification<Project>
    {
        private readonly int _projectId;

        public GetProjectById(int projectId)
        {
            _projectId = projectId;
        }

        public override Expression<Func<Project, bool>> AsExpression()
        {
            return project => project.ProjectId == _projectId;
        }
    }
}