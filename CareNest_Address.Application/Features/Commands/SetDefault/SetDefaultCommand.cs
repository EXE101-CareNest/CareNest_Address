using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Commands.SetDefault
{
    public class SetDefaultCommand : ICommand<Address>
    {
        // Address id to set default
        public string Id { get; set; } = string.Empty;
        // Optional account id; will be validated/used by handler
        public string? AccountId { get; set; }
        // Whether to set as default (true) or unset (false)
        public bool IsDefault { get; set; } = true;
    }
}
