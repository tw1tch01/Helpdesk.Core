using System.Collections.Generic;
using Helpdesk.Domain.Common;

namespace Helpdesk.Domain.Users.Events
{
    public class UserUpdatedEvent
    {
        public UserUpdatedEvent(int userId)
        {
            UserId = userId;
        }

        public UserUpdatedEvent(int userId, IReadOnlyDictionary<string, ValueChange> changes)
        {
            UserId = userId;
            Changes = changes;
        }

        public int UserId { get; }
        public IReadOnlyDictionary<string, ValueChange> Changes { get; }
    }
}