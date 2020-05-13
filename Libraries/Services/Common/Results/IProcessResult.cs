using System;

namespace Helpdesk.Services.Common.Results
{
    public interface IProcessResult<TResultEnum> where TResultEnum : Enum
    {
        TResultEnum Result { get; }
        string Message { get; }
    }
}