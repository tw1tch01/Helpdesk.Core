using System.Threading.Tasks;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common
{
    internal class EmptyEventService : IEventService
    {
        public Task Publish<TEvent>(TEvent @event) where TEvent : DomainEvent
        {
            return Task.CompletedTask;
        }
    }
}