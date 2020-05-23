using System;
using System.Collections.Generic;
using System.Linq;
using Data.Specifications;
using Helpdesk.Domain.Enums;
using Helpdesk.Domain.Tickets;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Enums;
using Helpdesk.Services.Common;
using Helpdesk.Services.Common.Specifications;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries
{
    public abstract class AbstractTicketsLookup : AbstractLookup<Ticket>
    {
        protected AbstractTicketsLookup(LinqSpecification<Ticket> defaultSpecifcation)
            : base(defaultSpecifcation)
        {
        }

        #region Methods

        protected AbstractTicketsLookup WithParameters(TicketLookupParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (parameters.CreatedAfter.HasValue) CreatedAfter(parameters.CreatedAfter.Value);

            if (parameters.CreatedBefore.HasValue) CreatedBefore(parameters.CreatedBefore.Value);

            if (!string.IsNullOrWhiteSpace(parameters.SearchBy)) SearchBy(parameters.SearchBy);

            if (parameters.TicketIds.Any()) WithinTicketIds(parameters.TicketIds);

            if (parameters.FilterByStatus.HasValue) FilterByStatus(parameters.FilterByStatus.Value);

            if (parameters.FilterBySeverity.HasValue) FilterBySeverity(parameters.FilterBySeverity.Value);

            if (parameters.FilterByPriority.HasValue) FilterByPriority(parameters.FilterByPriority.Value);

            if (parameters.SortBy.HasValue) SortBy(parameters.SortBy.Value);

            return this;
        }

        #endregion Methods

        #region Private Methods

        private void CreatedAfter(DateTimeOffset createdAfter)
        {
            And(new CreatedAfter<Ticket>(createdAfter));
        }

        private void CreatedBefore(DateTimeOffset createdBefore)
        {
            And(new CreatedBefore<Ticket>(createdBefore));
        }

        private void SearchBy(string searchBy)
        {
            And(new TicketNameContainsTerm(searchBy));
        }

        private void WithinTicketIds(IList<int> ticketIds)
        {
            And(new GetTicketsWithinIds(ticketIds));
        }

        private void FilterByStatus(TicketStatus status)
        {
            switch (status)
            {
                case TicketStatus.Open:
                    And(new GetOpenTickets());
                    break;

                case TicketStatus.Overdue:
                    And(new GetOverdueTickets());
                    break;

                case TicketStatus.Resolved:
                    And(new GetResolvedTickets());
                    break;

                case TicketStatus.Closed:
                    And(new GetClosedTickets());
                    break;

                case TicketStatus.InProgress:
                    And(new GetInProgressTickets());
                    break;

                case TicketStatus.OnHold:
                    And(new GetOnHoldTickets());
                    break;

                default:
                    throw new NotImplementedException(status.ToString());
            }
        }

        private void FilterBySeverity(Severity severity)
        {
            And(new GetTicketBySeverity(severity));
        }

        private void FilterByPriority(Priority priority)
        {
            And(new GetTicketByPriority(priority));
        }

        private void SortBy(SortTicketsBy sortBy)
        {
            switch (sortBy)
            {
                case SortTicketsBy.NameAsc:
                    _specification.OrderBy(ticket => ticket.Name);
                    break;

                case SortTicketsBy.NameDesc:
                    _specification.OrderByDescending(ticket => ticket.Name);
                    break;

                case SortTicketsBy.DueDateAsc:
                    _specification.OrderBy(ticket => ticket.DueDate);
                    break;

                case SortTicketsBy.DueDateDesc:
                    _specification.OrderByDescending(ticket => ticket.DueDate);
                    break;

                case SortTicketsBy.SeverityAsc:
                    _specification.OrderBy(ticket => ticket.Severity);
                    break;

                case SortTicketsBy.SeverityDesc:
                    _specification.OrderByDescending(ticket => ticket.Severity);
                    break;

                case SortTicketsBy.PriorityAsc:
                    _specification.OrderBy(ticket => ticket.Priority);
                    break;

                case SortTicketsBy.PriorityDesc:
                    _specification.OrderByDescending(ticket => ticket.Priority);
                    break;
            }
        }

        #endregion Private Methods
    }
}