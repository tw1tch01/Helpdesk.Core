using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Entities;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Projects.Specifications
{
    public class GetProjectById : LinqSpecification<Project>
    {
        internal readonly int _projectId;

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