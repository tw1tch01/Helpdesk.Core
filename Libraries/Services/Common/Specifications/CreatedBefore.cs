using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedBefore<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        public CreatedBefore(DateTimeOffset createdBefore)
        {
            CreatedDate = createdBefore;
        }

        public DateTimeOffset CreatedDate { get; }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.CreatedOn <= CreatedDate;
        }
    }
}