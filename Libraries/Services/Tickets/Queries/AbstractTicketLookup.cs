using System;
using System.Collections.Generic;
using System.Linq;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.Domain.Enums;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.DomainModels.Tickets.Enums;
using Helpdesk.Services.Common.Specifications;
using Helpdesk.Services.Tickets.Specifications;

namespace Helpdesk.Services.Tickets.Queries
{
    public abstract class AbstractTicketsLookup
    {
        protected LinqSpecification<Ticket> _specification;

        protected AbstractTicketsLookup(LinqSpecification<Ticket> defaultSpecifcation)
        {
            _specification = defaultSpecifcation;
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
            AndSpecification(new CreatedAfter<Ticket>(createdAfter));
        }

        private void CreatedBefore(DateTimeOffset createdBefore)
        {
            AndSpecification(new CreatedBefore<Ticket>(createdBefore));
        }

        private void SearchBy(string searchBy)
        {
            AndSpecification(new TicketNameContainsTerm(searchBy));
        }

        private void WithinTicketIds(IList<int> ticketIds)
        {
            AndSpecification(new GetTicketByIds(ticketIds));
        }

        private void FilterByStatus(TicketStatus status)
        {
            switch (status)
            {
                case TicketStatus.Open:
                    AndSpecification(new GetOpenTickets());
                    break;

                case TicketStatus.Overdue:
                    AndSpecification(new GetOverdueTickets());
                    break;

                case TicketStatus.Resolved:
                    AndSpecification(new GetResolvedTickets());
                    break;

                case TicketStatus.Closed:
                    AndSpecification(new GetClosedTickets());
                    break;

                case TicketStatus.PendingApproval:
                    AndSpecification(new GetPendingApprovalTickets());
                    break;

                case TicketStatus.Approved:
                    AndSpecification(new GetApprovedTickets());
                    break;

                case TicketStatus.PendingFeedback:
                    AndSpecification(new GetPendingFeedbackTickets());
                    break;

                case TicketStatus.InProgress:
                    AndSpecification(new GetInProgressTickets());
                    break;

                case TicketStatus.OnHold:
                    AndSpecification(new GetOnHoldTickets());
                    break;
            }
        }

        private void FilterBySeverity(Severity severity)
        {
            AndSpecification(new GetTicketBySeverity(severity));
        }

        private void FilterByPriority(Priority priority)
        {
            AndSpecification(new GetTicketByPriority(priority));
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

        private void AndSpecification(LinqSpecification<Ticket> filterSpec)
        {
            if (_specification == null) _specification = filterSpec;
            else _specification &= filterSpec;
        }

        #endregion Private Methods
    }
}