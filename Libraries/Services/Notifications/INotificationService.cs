using System.Threading.Tasks;

namespace Helpdesk.Services.Notifications
{
    public interface INotificationService
    {
        Task Queue(INotificationProcess notification);
    }
}