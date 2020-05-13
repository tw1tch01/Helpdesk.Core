using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Common.Results
{
    public interface IWorkflowResult
    {
        IWorkflowProcess Workflow { get; }
    }
}