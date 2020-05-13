using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class DeleteTicketResult : IProcessResult<TicketDeleteResult>, IWorkflowResult
    {
        public DeleteTicketResult(TicketDeleteResult result)
        {
            Result = result;
        }

        public TicketDeleteResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public IWorkflowProcess Workflow { get; private set; }

        #region Methods

        internal static DeleteTicketResult TicketNotFound(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static DeleteTicketResult Deleted(int ticketId)
        {
            return new DeleteTicketResult(TicketDeleteResult.Deleted)
            {
                TicketId = ticketId
            };
        }

        internal static DeleteTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new DeleteTicketResult(TicketDeleteResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        private string GetMessage() => Result switch
        {
            TicketDeleteResult.Deleted => ResultMessages.Deleted,
            TicketDeleteResult.TicketNotFound => ResultMessages.TicketNotFound,
            TicketDeleteResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}