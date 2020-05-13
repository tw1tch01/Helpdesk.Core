﻿using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.PauseTicket
{
    public class BeforeTicketPausedWorkflow : TicketPausedEvent, IWorkflowProcess
    {
        public BeforeTicketPausedWorkflow(int ticketId)
            : base(ticketId)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}