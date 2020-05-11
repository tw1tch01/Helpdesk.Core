using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Helpdesk.Services.Notifications
{
    public class NotificationService : Mediator, INotificationService
    {
        public NotificationService(ServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }

        public async Task Queue(INotificationProcess notification)
        {
            if (notification is INotification)
            {
                await Publish(notification);
            }
        }

        protected override Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
        {
            return ParallelNoWait(allHandlers, notification, cancellationToken);
        }

        private static Task ParallelNoWait(IEnumerable<Func<INotification, CancellationToken, Task>> handlers, INotification notification, CancellationToken cancellationToken)
        {
            foreach (var handler in handlers)
            {
                Task.Run(() => handler(notification, cancellationToken));
            }

            return Task.CompletedTask;
        }
    }
}