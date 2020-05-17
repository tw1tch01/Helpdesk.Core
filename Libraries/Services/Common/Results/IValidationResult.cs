using System.Collections.Generic;

namespace Helpdesk.Services.Common.Results
{
    public interface IValidationResult
    {
        Dictionary<string, List<string>> ValidationFailures { get; }
    }
}