using System;
using Data.Specifications;

namespace Helpdesk.Services.Common
{
    public abstract class AbstractLookup<TEntity> where TEntity : class
    {
        protected LinqSpecification<TEntity> _specification;
        protected int _defaultPageSize = 25;
        protected int _maximumPageSize = 100;

        protected AbstractLookup(LinqSpecification<TEntity> specification)
        {
            _specification = specification;
        }

        protected void And(LinqSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            if (_specification == null) _specification = specification;
            else _specification &= specification;
        }

        protected void AndNot(LinqSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            if (_specification == null) _specification = !specification;
            else _specification &= !specification;
        }

        protected void Or(LinqSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            if (_specification == null) _specification = specification;
            else _specification |= specification;
        }

        protected void OrNot(LinqSpecification<TEntity> specification)
        {
            if (specification == null) throw new ArgumentNullException(nameof(specification));

            if (_specification == null) _specification = !specification;
            else _specification |= !specification;
        }

        protected (int page, int pageSize) ValidatePaging(int page, int pageSize)
        {
            if (page < 0) page = 0;

            if (pageSize <= 0) pageSize = _defaultPageSize;
            else if (pageSize > _maximumPageSize) pageSize = _maximumPageSize;

            return (page, pageSize);
        }
    }
}