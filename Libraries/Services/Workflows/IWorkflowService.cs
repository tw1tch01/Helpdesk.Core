using System.Threading.Tasks;
using Helpdesk.Services.Common.Results;

namespace Helpdesk.Services.Workflows
{
    public interface IWorkflowService
    {
        Task Process(IWorkflowProcess request);
    }
}