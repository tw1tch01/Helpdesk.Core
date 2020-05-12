using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class ModifiedBy<T> : LinqSpecification<T> where T : class, IModifiedAudit
    {
        private readonly int _modifiedBy;

        public ModifiedBy(int modifiedBy)
        {
            _modifiedBy = modifiedBy;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.ModifiedBy == _modifiedBy;
        }
    }
}