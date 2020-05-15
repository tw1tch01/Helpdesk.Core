using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class ReopenTicketResult : IProcessResult<TicketReopenResult>, IWorkflowResult
    {
        public ReopenTicketResult(TicketReopenResult result)
        {
            Result = result;
        }

        public TicketReopenResult Result { get; }

        public string Message => GetMessage();

        public int TicketId { get; internal set; }

        public int? UserId { get; internal set; }

        public IWorkflowProcess Workflow { get; internal set; }

        #region Methods

        internal static ReopenTicketResult TicketNotFound(int ticketId)
        {
            return new ReopenTicketResult(TicketReopenResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static ReopenTicketResult Reopened(int ticketId)
        {
            return new ReopenTicketResult(TicketReopenResult.Reopened)
            {
                TicketId = ticketId
            };
        }

        internal static ReopenTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new ReopenTicketResult(TicketReopenResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        private string GetMessage() => Result switch
        {
            TicketReopenResult.Reopened => ResultMessages.Reopened,
            TicketReopenResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketReopenResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}