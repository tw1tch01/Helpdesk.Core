using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class CreatedProcess<T> : LinqSpecification<T> where T : class, ICreatedAudit
    {
        public CreatedProcess(string createdProcess)
        {
            if (string.IsNullOrWhiteSpace(createdProcess)) throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(createdProcess));

            Process = createdProcess;
        }

        public string Process { get; }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => !string.IsNullOrWhiteSpace(entity.CreatedProcess) && entity.CreatedProcess.Equals(Process);
        }
    }
}