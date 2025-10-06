using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Commands.Update
{
    public class UpdateCommand : ICommand<Address>
    {
        public string Id { get; set; } = string.Empty;
        public string? ReceiverName { get; set; }
        public string? AddressName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
    }
}