﻿using System;
using Helpdesk.DomainModels.Tickets.Events;
using Helpdesk.Services.Workflows;
using Helpdesk.Services.Workflows.Enums;

namespace Helpdesk.Services.Tickets.Events.ReopenTicket
{
    public class BeforeTicketReopenedWorkflow : TicketReopenedEvent, IWorkflowProcess
    {
        public BeforeTicketReopenedWorkflow(int ticketId, Guid userGuid)
            : base(ticketId, userGuid)
        {
        }

        public virtual WorkflowResult Result { get; set; }
        public virtual string Message { get; set; }
    }
}