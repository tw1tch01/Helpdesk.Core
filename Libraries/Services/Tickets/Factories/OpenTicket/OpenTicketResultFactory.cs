using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.OpenTicket
{
    public class OpenTicketResultFactory : IOpenTicketResultFactory
    {
        public OpenTicketResult ClientNotFound(int clientId)
        {
            return new OpenTicketResult(TicketOpenResult.ClientNotFound)
            {
                ClientId = clientId
            };
        }

        public OpenTicketResult Opened(Ticket ticket)
        {
            return new OpenTicketResult(TicketOpenResult.Opened)
            {
                TicketId = ticket.TicketId,
                ClientId = ticket.ClientId
            };
        }

        public OpenTicketResult ProjectInaccessible(int clientId, int projectId)
        {
            return new OpenTicketResult(TicketOpenResult.ProjectInaccessible)
            {
                ClientId = clientId,
                ProjectId = projectId
            };
        }

        public OpenTicketResult ProjectNotFound(int projectId)
        {
            return new OpenTicketResult(TicketOpenResult.ProjectNotFound)
            {
                ProjectId = projectId
            };
        }

        public virtual OpenTicketResult ValidationFailure(IList<ValidationFailure> errors)
        {
            return new OpenTicketResult(TicketOpenResult.ValidationFailure)
            {
                ValidationFailures = errors.GroupPropertyWithErrors()
            };
        }
    }
}