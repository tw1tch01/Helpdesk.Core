using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Common.Results;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class OpenTicketResult : IProcessResult<TicketOpenResult>, IValidationResult
    {
        public OpenTicketResult(TicketOpenResult result)
        {
            Result = result;
        }

        public TicketOpenResult Result { get; }
        public string Message => GetMessage();
        public int? TicketId { get; set; }
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }
        public Dictionary<string, List<string>> ValidationFailures { get; private set; }

        #region Methods

        internal static OpenTicketResult ValidationFailure(IList<ValidationFailure> errors)
        {
            return new OpenTicketResult(TicketOpenResult.ValidationFailure)
            {
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }

        internal static OpenTicketResult ClientNotFound(int clientId)
        {
            return new OpenTicketResult(TicketOpenResult.ClientNotFound)
            {
                ClientId = clientId
            };
        }

        internal static OpenTicketResult ProjectNotFound(int projectId)
        {
            return new OpenTicketResult(TicketOpenResult.ProjectNotFound)
            {
                ProjectId = projectId
            };
        }

        internal static OpenTicketResult ProjectInaccessible(int clientId, int projectId)
        {
            return new OpenTicketResult(TicketOpenResult.ProjectInaccessible)
            {
                ClientId = clientId,
                ProjectId = projectId
            };
        }

        internal static OpenTicketResult Opened(Ticket ticket)
        {
            return new OpenTicketResult(TicketOpenResult.Opened)
            {
                TicketId = ticket.TicketId,
                ClientId = ticket.ClientId,
                ProjectId = ticket.ProjectId
            };
        }

        private string GetMessage() => Result switch
        {
            TicketOpenResult.Opened => ResultMessages.Opened,
            TicketOpenResult.ValidationFailure => ResultMessages.ValidationFailure,
            TicketOpenResult.ClientNotFound => ResultMessages.ClientNotFound,
            TicketOpenResult.ProjectNotFound => ResultMessages.ProjectNotFound,
            TicketOpenResult.ProjectInaccessible => ResultMessages.ProjectInaccessible,
            _ => Result.ToString()
        };

        #endregion Methods
    }
}