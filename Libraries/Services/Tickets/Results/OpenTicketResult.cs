using System.Collections.Generic;
using FluentValidation.Results;
using Helpdesk.Services.Extensions;
using Helpdesk.Services.Tickets.Results.Enums;

namespace Helpdesk.Services.Tickets.Results
{
    public class OpenTicketResult
    {
        public OpenTicketResult(TicketOpenResult result)
        {
            Result = result;
        }

        public OpenTicketResult(IList<ValidationFailure> failures)
        {
            Result = TicketOpenResult.ValidationFailure;
            Failures = failures.GroupPropertyWithErrors();
        }

        public TicketOpenResult Result { get; }
        public string Message => GetMessage();
        public int TicketId { get; set; }
        public int ClientId { get; set; }
        public int? ProjectId { get; set; }
        public Dictionary<string, List<string>> Failures { get; } = new Dictionary<string, List<string>>();

        public string GetMessage()
        {
            return Result switch
            {
                TicketOpenResult.Opened => TicketResultMessages.Opened,
                TicketOpenResult.ValidationFailure => TicketResultMessages.ValidationFailure,
                TicketOpenResult.ClientNotFound => TicketResultMessages.ClientNotFound,
                TicketOpenResult.ProjectNotFound => TicketResultMessages.ProjectNotFound,
                TicketOpenResult.ProjectInaccessible => TicketResultMessages.ProjectInaccessible,
                _ => Result.ToString()
            };
        }
    }
}