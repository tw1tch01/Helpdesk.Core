using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common.Results
{
    public interface IUpdateResult
    {
        IReadOnlyDictionary<string, ValueChange> PropertyChanges { get; }
    }
}