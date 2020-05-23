namespace Helpdesk.Services.Tickets.Results.Enums
{
    public enum TicketAssignResult
    {
        Assigned,
        TicketNotFound,
        TicketAlreadyResolved,
        TicketAlreadyClosed,
        WorkflowFailed
    }
}