using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Common;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results.Enums;
using Helpdesk.Services.Workflows;

namespace Helpdesk.Services.Tickets.Results
{
    public class UpdateTicketResult : IProcessResult<TicketUpdateResult>, IValidationResult, IUpdateResult, IWorkflowResult
    {
        public UpdateTicketResult(TicketUpdateResult result)
        {
            Result = result;
        }

        public TicketUpdateResult Result { get; }
        public string Message => GetMessage();

        public int TicketId { get; private set; }
        public Dictionary<string, List<string>> ValidationFailures { get; private set; }
        public IReadOnlyDictionary<string, ValueChange> PropertyChanges { get; private set; }
        public IWorkflowProcess Workflow { get; private set; }

        #region Methods

        internal static UpdateTicketResult ValidationFailure(int ticketId, IList<ValidationFailure> errors)
        {
            return new UpdateTicketResult(TicketUpdateResult.ValidationFailure)
            {
                TicketId = ticketId,
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }

        internal static UpdateTicketResult TicketNotFound(int ticketId)
        {
            return new UpdateTicketResult(TicketUpdateResult.TicketNotFound)
            {
                TicketId = ticketId
            };
        }

        internal static UpdateTicketResult Updated(Ticket ticket, IReadOnlyDictionary<string, ValueChange> changes)
        {
            return new UpdateTicketResult(TicketUpdateResult.Updated)
            {
                TicketId = ticket.TicketId,
                PropertyChanges = changes
            };
        }

        internal static UpdateTicketResult WorkflowFailed(int ticketId, IWorkflowProcess workflow)
        {
            return new UpdateTicketResult(TicketUpdateResult.WorkflowFailed)
            {
                TicketId = ticketId,
                Workflow = workflow
            };
        }

        private string GetMessage() => Result switch
        {
            TicketUpdateResult.Updated => ResultMessages.Updated,
            TicketUpdateResult.ValidationFailure => ResultMessages.ValidationFailure,
            TicketUpdateResult.TicketNotFound => ResultMessages.TicketNotFound,
            _ => Result.ToString(),
        };

        #endregion Methods
    }
}