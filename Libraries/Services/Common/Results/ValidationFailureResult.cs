using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Helpdesk.Services.Common.Results
{
    public class ValidationFailureResult : ProcessResult
    {
        protected const string _failuresKey = "Failures";
        private const string _message = "One or more validation failures have occurred.";

        public ValidationFailureResult(ICollection<ValidationFailure> failures)
            : base(false, _message)
        {
            Data[_failuresKey] = GetValidationFailures(failures);
        }

        public static Dictionary<string, List<string>> GetValidationFailures(ICollection<ValidationFailure> failures)
        {
            if (!failures?.Any() ?? true) return new Dictionary<string, List<string>>();

            return (from failure in failures
                    group failure by failure.PropertyName into gr
                    let errors = gr.Select(a => a.ErrorMessage).Distinct()
                    where errors != null && errors.Any()
                    select new KeyValuePair<string, IEnumerable<string>>(gr.Key, errors)).ToDictionary(k => k.Key, v => v.Value.ToList());
        }
    }
}