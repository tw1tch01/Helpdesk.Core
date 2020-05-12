using System;
using Data.Specifications;
using Helpdesk.Domain.Entities;
using Helpdesk.DomainModels.Tickets;
using Helpdesk.Services.Common.Specifications;

namespace Helpdesk.Services.UserTickets.Queries
{
    public abstract class AbstractUserTicketsLookup
    {
        protected LinqSpecification<UserTicket> _specification;

        protected AbstractUserTicketsLookup()
        {
        }

        protected AbstractUserTicketsLookup(LinqSpecification<UserTicket> defaultSpecifcation)
        {
            _specification = defaultSpecifcation;
        }

        #region Methods

        protected AbstractUserTicketsLookup WithParameters(TicketLookupParams parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            if (parameters.CreatedAfter.HasValue) CreatedAfter(parameters.CreatedAfter.Value);

            if (parameters.CreatedBefore.HasValue) CreatedBefore(parameters.CreatedBefore.Value);

            //if (!string.IsNullOrWhiteSpace(parameters.SearchBy)) SearchBy(parameters.SearchBy);

            //if (parameters.TicketIds.Any()) WithinTicketIds(parameters.TicketIds);

            //if (parameters.FilterByStatus.HasValue) FilterByStatus(parameters.FilterByStatus.Value);

            //if (parameters.FilterBySeverity.HasValue) FilterBySeverity(parameters.FilterBySeverity.Value);

            //if (parameters.FilterByPriority.HasValue) FilterByPriority(parameters.FilterByPriority.Value);

            //if (parameters.SortBy.HasValue) SortBy(parameters.SortBy.Value);

            return this;
        }

        #endregion Methods

        #region Private Methods

        private void CreatedAfter(DateTimeOffset createdAfter)
        {
            AndSpecification(new CreatedAfter<UserTicket>(createdAfter));
        }

        private void CreatedBefore(DateTimeOffset createdBefore)
        {
            AndSpecification(new CreatedBefore<UserTicket>(createdBefore));
        }

        private void AndSpecification(LinqSpecification<UserTicket> filterSpec)
        {
            if (_specification == null) _specification = filterSpec;
            else _specification &= filterSpec;
        }

        #endregion Private Methods
    }
}