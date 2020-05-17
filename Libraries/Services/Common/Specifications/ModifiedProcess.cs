using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Specifications
{
    public class ModifiedProcess<T> : LinqSpecification<T> where T : class, IModifiedAudit
    {
        private readonly string _modifiedProcess;

        public ModifiedProcess(string modifiedProcess)
        {
            if (string.IsNullOrWhiteSpace(modifiedProcess)) throw new ArgumentException("Value cannot be null, empty or whitespace.", nameof(modifiedProcess));

            _modifiedProcess = modifiedProcess;
        }

        public override Expression<Func<T, bool>> AsExpression()
        {
            return entity => !string.IsNullOrWhiteSpace(entity.ModifiedProcess) && entity.ModifiedProcess.Equals(_modifiedProcess);
        }
    }
}