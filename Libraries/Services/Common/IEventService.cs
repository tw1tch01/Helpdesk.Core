using System.Threading.Tasks;
using Helpdesk.Domain.Common;

namespace Helpdesk.Services.Common
{
    public interface IEventService
    {
        /// <summary>
        /// Publish an event that has occurred
        /// </summary>
        /// <typeparam name="TEvent">Event type</typeparam>
        /// <param name="event">Event</param>
        /// <returns></returns>
        Task Publish<TEvent>(TEvent @event) where TEvent : DomainEvent;
    }
}