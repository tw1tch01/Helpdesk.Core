namespace Helpdesk.Services.Tickets.Results
{
    internal static class ResultMessages
    {
        public const string ClientNotFound = "Client record was not found.";

        public const string Closed = "Ticket has been closed.";

        public const string Deleted = "Ticket has been deleted.";

        public const string Opened = "Ticket has been opened.";

        public const string Paused = "Ticket has been paused.";

        public const string ProjectNotFound = "Project record was not found.";

        public const string ProjectInaccessible = "Project is inaccessible to Client.";

        public const string Reopened = "Ticket has been reopened.";

        public const string Resolved = "Ticket has been resolved.";

        public const string Started = "Ticket has been started.";

        public const string TicketNotFound = "Ticket record was not found.";

        public const string TicketAlreadyClosed = "Ticket has already been closed.";

        public const string TicketAlreadyPaused = "Ticket has already been paused.";

        public const string TicketAlreadyResolved = "Ticket has already been resolved.";

        public const string TicketAlreadyStarted = "Ticket has already been started.";

        public const string Updated = "Ticket has been updated.";

        public const string UserNotFound = "User record was not found.";

        public const string ValidationFailure = "One or more validation failures have occurred.";

        public const string WorkflowFailed = "Workflow has failed.";
    }
}