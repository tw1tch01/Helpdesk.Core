using System.Threading.Tasks;
using MediatR;

namespace Helpdesk.Services.Workflows
{
    public class WorkflowService : Mediator, IWorkflowService
    {
        public WorkflowService(ServiceFactory serviceFactory)
            : base(serviceFactory)
        {
        }

        public async Task<IWorkflowProcess> Process(IWorkflowProcess request)
        {
            if (request is INotification)
            {
                await Publish(request);
            }

            return request;
        }
    }
}