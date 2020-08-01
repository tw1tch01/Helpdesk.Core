using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedAfter<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        public CreatedAfter(DateTimeOffset createdAfter)
        {
            CreatedDate = createdAfter;
        }

        public DateTimeOffset CreatedDate { get; }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return a => a.CreatedOn > CreatedDate;
        }
    }
}