using System.Collections.Generic;

namespace Helpdesk.Services.Common.Results
{
    public abstract class ProcessResult
    {
        protected ProcessResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
            Data = new Dictionary<string, object>();
        }

        public bool IsValid { get; }
        public string Message { get; }
        public IDictionary<string, object> Data { get; protected set; }
    }
}