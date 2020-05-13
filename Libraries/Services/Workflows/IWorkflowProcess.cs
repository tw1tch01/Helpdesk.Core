using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Workflows.Enums;
using MediatR;

namespace Helpdesk.Services.Workflows
{
    public interface IWorkflowProcess : INotification, IProcessResult<WorkflowResult>
    {
    }
}