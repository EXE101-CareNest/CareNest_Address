using CareNest_Address.Domain.Commons.Base;

namespace CareNest_Address.Domain.Entitites
{
    public class Address : BaseEntity
    {
        public string? ReceiverName { get; set; }
        public string? AddressName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
        public bool? IsDefault { get; set; }
    }
}
