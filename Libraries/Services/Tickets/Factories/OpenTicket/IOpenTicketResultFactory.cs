using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Entities;
using Helpdesk.Services.Tickets.Results;

namespace Helpdesk.Services.Tickets.Factories.OpenTicket
{
    public interface IOpenTicketResultFactory
    {
        OpenTicketResult Opened(Ticket ticket);

        OpenTicketResult ValidationFailure(IList<ValidationFailure> errors);
    }
}