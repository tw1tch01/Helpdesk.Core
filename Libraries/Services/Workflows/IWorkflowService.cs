using System.Threading.Tasks;

namespace Helpdesk.Services.Workflows
{
    public interface IWorkflowService
    {
        Task Process(IWorkflowProcess request);
    }
}