using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;

namespace Helpdesk.Services.Tickets.Specifications
{
    public class GetTicketByStatus : LinqSpecification<Ticket>
    {
        private readonly TicketStatus _status;

        public GetTicketByStatus(TicketStatus status)
        {
            _status = status;
        }

        public override Expression<Func<Ticket, bool>> AsExpression()
        {
            switch (_status)
            {
                case TicketStatus.Open:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && !ticket.ApprovalRequestedOn.HasValue
                                  && !ticket.FeedbackRequestedOn.HasValue
                                  && !ticket.PausedOn.HasValue
                                  && ticket.StartedOn.HasValue;

                case TicketStatus.Overdue:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && !ticket.ApprovalRequestedOn.HasValue
                                  && !ticket.FeedbackRequestedOn.HasValue
                                  && !ticket.PausedOn.HasValue
                                  && !ticket.StartedOn.HasValue
                                  && ticket.DueDate < DateTimeOffset.UtcNow;

                case TicketStatus.Resolved:
                    return ticket => ticket.ResolvedOn.HasValue;

                case TicketStatus.Closed:
                    return ticket => ticket.ClosedOn.HasValue;

                case TicketStatus.PendingApproval:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && ticket.ApprovalRequestedOn.HasValue;

                case TicketStatus.Approved:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && ticket.ApprovedOn.HasValue;

                case TicketStatus.PendingFeedback:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && !ticket.ApprovalRequestedOn.HasValue
                                  && ticket.FeedbackRequestedOn.HasValue;

                case TicketStatus.InProgress:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && !ticket.ApprovalRequestedOn.HasValue
                                  && !ticket.FeedbackRequestedOn.HasValue
                                  && !ticket.PausedOn.HasValue
                                  && ticket.StartedOn.HasValue;

                case TicketStatus.OnHold:
                    return ticket => !ticket.ResolvedOn.HasValue
                                  && !ticket.ClosedOn.HasValue
                                  && !ticket.ApprovedOn.HasValue
                                  && !ticket.ApprovalRequestedOn.HasValue
                                  && !ticket.FeedbackRequestedOn.HasValue
                                  && ticket.PausedOn.HasValue;

                default:
                    throw new NotImplementedException($"Expression for status {_status} has not been implemented yet.");
            }
        }
    }
}