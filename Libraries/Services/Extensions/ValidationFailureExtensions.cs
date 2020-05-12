using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Helpdesk.Services.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static Dictionary<string, List<string>> GroupPropertyWithErrors(this IList<ValidationFailure> failures)
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