using Helpdesk.Services.Common.Results;
using Helpdesk.Services.TicketLinks.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.TicketLinks.Results
{
    public class UnlinkTicketsResult : IProcessResult<TicketsUnlinkResult>, IWorkflowResult
    {
        public UnlinkTicketsResult(TicketsUnlinkResult result)
        {
            Result = result;
        }

        public TicketsUnlinkResult Result { get; }
        public string Message => GetMessage();
        public int FromTicketId { get; set; }
        public int ToTicketId { get; set; }
        public IWorkflowProcess Workflow { get; set; }

        #region Methods

        private string GetMessage() => Result switch
        {
            TicketsUnlinkResult.Unlinked => ResultMessages.Unlinked,
            TicketsUnlinkResult.TicketsNotLinked => ResultMessages.TicketsNotLinked,
            TicketsUnlinkResult.WorkflowFailed => ResultMessages.WorkflowFailed,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}