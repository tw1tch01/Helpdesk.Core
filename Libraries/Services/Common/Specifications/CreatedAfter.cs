using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedAfter<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        private readonly DateTimeOffset _createdAfter;

        public CreatedAfter(DateTimeOffset createdAfter)
        {
            _createdAfter = createdAfter;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return a => a.CreatedOn > _createdAfter;
        }
    }
}