﻿using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.DeleteTicket
{
    public class BeforeTicketDeletedWorkflow : TicketDeletedEvent, IWorkflowProcess
    {
        public BeforeTicketDeletedWorkflow(int ticketId)
            : base(ticketId)
        {
        }

        public WorkflowResult Result { get; set; }
        public string Message { get; set; }
    }
}