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

            return failures.GroupBy(g => g.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(v => v.ErrorMessage).Distinct().ToList());
        }
    }
}