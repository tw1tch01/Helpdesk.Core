using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Domain.Tickets;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Factories.OpenTicket
{
    public class OpenTicketResultFactory : IOpenTicketResultFactory
    {
        public OpenTicketResult Opened(Ticket ticket)
        {
            return new OpenTicketResult(TicketOpenResult.Opened)
            {
                TicketId = ticket.TicketId,
                UserGuid = ticket.UserGuid
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