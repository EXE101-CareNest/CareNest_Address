
using CareNest_Address.Application.Interfaces.CQRS.Commands;

namespace CareNest_Address.Application.Features.Commands.Delete
{
    public class DeleteCommand : ICommand
    {
        public required string Id { get; set; }
    }
}
