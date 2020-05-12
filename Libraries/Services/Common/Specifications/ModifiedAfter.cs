using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class ModifiedAfter<T> : LinqSpecification<T> where T : class, IModifiedAudit
    {
        private readonly DateTime _modifiedAfter;

        public ModifiedAfter(DateTime modifiedAfter)
        {
            _modifiedAfter = modifiedAfter;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return a => a.ModifiedOn > _modifiedAfter;
        }
    }
}