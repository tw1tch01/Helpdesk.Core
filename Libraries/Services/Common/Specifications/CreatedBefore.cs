using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedBefore<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        private readonly DateTimeOffset _createdBefore;

        public CreatedBefore(DateTimeOffset createdBefore)
        {
            _createdBefore = createdBefore;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.CreatedOn <= _createdBefore;
        }
    }
}