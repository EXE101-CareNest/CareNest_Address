namespace CareNest_Address.Application.Features.Commands.Update
{
    public class UpdateRequest
    {
        public string? ReceiverName { get; set; }
        public string? AddressName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
    }
}
