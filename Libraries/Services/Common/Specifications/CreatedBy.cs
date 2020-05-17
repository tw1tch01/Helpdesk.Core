using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedBy<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        private readonly int _createdBy;

        public CreatedBy(int createdBy)
        {
            _createdBy = createdBy;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.CreatedBy == _createdBy;
        }
    }
}