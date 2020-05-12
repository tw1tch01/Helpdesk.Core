using System;
using System.Linq.Expressions;
using Data.Specifications;
using Helpdesk.Domain.Entities;

namespace Helpdesk.Services.Clients.Specifications
{
    public class GetClientById : LinqSpecification<Client>
    {
        private readonly int _clientId;

        public GetClientById(int clientId)
        {
            _clientId = clientId;
        }

        public override Expression<Func<Client, bool>> AsExpression()
        {
            return client => client.ClientId == _clientId;
        }
    }
}