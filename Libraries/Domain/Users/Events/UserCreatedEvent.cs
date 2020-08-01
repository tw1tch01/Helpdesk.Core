namespace Helpdesk.Domain.Users.Events
{
    public class UserCreatedEvent
    {
        public UserCreatedEvent(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}