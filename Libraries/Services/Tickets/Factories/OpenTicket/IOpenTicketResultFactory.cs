using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.OpenTicket
{
    public interface IOpenTicketResultFactory
    {
        OpenTicketResult ClientNotFound(int clientId);

        OpenTicketResult Opened(Ticket ticket);

        OpenTicketResult ProjectInaccessible(int clientId, int projectId);

        OpenTicketResult ProjectNotFound(int projectId);

        OpenTicketResult ValidationFailure(IList<ValidationFailure> errors);
    }
}