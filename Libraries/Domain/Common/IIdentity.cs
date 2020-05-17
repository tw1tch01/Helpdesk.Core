using System;

namespace Helpdesk.Domain.Common
{
    public interface IIdentity
    {
        Guid Identifier { get; set; }
    }
}