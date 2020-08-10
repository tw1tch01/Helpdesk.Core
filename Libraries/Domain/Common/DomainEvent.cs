using System;
using MediatR;

namespace Helpdesk.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            RaisedOn = DateTimeOffset.Now;
        }

        public DateTimeOffset RaisedOn { get; }

        public DateTimeOffset? HandledOn { get; private set; }

        public void Handled()
        {
            HandledOn = DateTimeOffset.Now;
        }
    }
}