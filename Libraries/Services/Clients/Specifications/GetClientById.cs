using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Data.Specifications;
using Helpdesk.Domain.Entities;

[assembly: InternalsVisibleTo("Helpdesk.Services.UnitTests")]

namespace Helpdesk.Services.Clients.Specifications
{
    public class GetClientById : LinqSpecification<Client>
    {
        internal readonly int _clientId;

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