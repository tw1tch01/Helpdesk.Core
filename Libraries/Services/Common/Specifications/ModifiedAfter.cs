using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class ModifiedAfter<T> : LinqSpecification<T> where T : class, IModifiedAudit
    {
        public ModifiedAfter(DateTimeOffset modifiedAfter)
        {
            ModifiedDate = modifiedAfter;
        }

        public DateTimeOffset ModifiedDate { get; }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return a => a.ModifiedOn > ModifiedDate;
        }
    }
}