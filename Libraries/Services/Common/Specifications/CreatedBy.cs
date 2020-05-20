using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedBy<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        private readonly string _createdBy;

        public CreatedBy(string createdBy)
        {
            if (string.IsNullOrWhiteSpace(createdBy)) throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(createdBy));

            _createdBy = createdBy;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => entity.CreatedBy == _createdBy;
        }
    }
}