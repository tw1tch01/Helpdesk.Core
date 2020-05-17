namespace Helpdesk.Domain.Enums
{
    public enum TicketStatus
    {
        Open = 0,
        Overdue = 1,
        Resolved = 2,
        Closed = 3,
        PendingApproval = 4,
        Approved = 5,
        PendingFeedback = 6,
        InProgress = 7,
        OnHold = 8
    }
}