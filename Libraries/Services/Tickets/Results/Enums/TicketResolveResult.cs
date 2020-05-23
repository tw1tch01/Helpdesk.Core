namespace Helpdesk.Services.Tickets.Results.Enums
{
    public enum TicketResolveResult
    {
        Resolved,
        TicketNotFound,
        TicketAlreadyResolved,
        TicketAlreadyClosed,
        WorkflowFailed,
    }
}