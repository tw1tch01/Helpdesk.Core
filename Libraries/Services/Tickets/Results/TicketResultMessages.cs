namespace Helpdesk.Services.Tickets.Results
{
    internal static class TicketResultMessages
    {
        public const string ClientNotFound = "Client record was not found.";
        public const string Closed = "Ticket has been closed.";
        public const string Deleted = "Ticket has been deleted.";
        public const string Opened = "Ticket has been opened.";
        public const string ProjectNotFound = "Project record was not found.";
        public const string ProjectInaccessible = "Project is inaccessible to Client.";
        public const string TicketNotFound = "Ticket record was not found.";
        public const string TicketAlreadyResolved = "Ticket has already been resolved.";
        public const string TicketAlreadyClosed = "Ticket has already been closed.";
        public const string UserNotFound = "User record was not found.";
        public const string ValidationFailure = "One or more validation failures have occurred.";
    }
}