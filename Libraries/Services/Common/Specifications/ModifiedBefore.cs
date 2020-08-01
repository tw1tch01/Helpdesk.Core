using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class ModifiedBefore<T> : LinqSpecification<T> where T : class, IModifiedAudit
    {
        public ModifiedBefore(DateTimeOffset modifiedBefore)
        {
            ModifiedDate = modifiedBefore;
        }

        public DateTimeOffset ModifiedDate { get; }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.ModifiedOn <= ModifiedDate;
        }
    }
}