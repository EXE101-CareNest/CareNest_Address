using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Commands.Create
{
    public class CreateCommand : ICommand<Address>
    {
        public string? ReceiverName { get; set; }
        public string? AddressName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
    }
}
