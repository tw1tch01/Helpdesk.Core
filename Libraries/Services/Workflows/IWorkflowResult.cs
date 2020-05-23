namespace Helpdesk.Services.Workflows
{
    public interface IWorkflowResult
    {
        IWorkflowProcess Workflow { get; set; }
    }
}