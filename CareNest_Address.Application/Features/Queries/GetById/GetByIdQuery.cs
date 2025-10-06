using CareNest_Address.Application.Interfaces.CQRS.Queries;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Queries.GetById
{
    public class GetByIdQuery : IQuery<Address>
    {
        public required string Id { get; set; }
    }
}
