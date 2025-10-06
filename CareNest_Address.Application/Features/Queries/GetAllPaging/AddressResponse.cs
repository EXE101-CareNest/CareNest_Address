namespace CareNest_Address.Application.Features.Queries.GetAllPaging
{
    public class AddressResponse
    {
        /// <summary>
        /// Id chi nhánh 
        /// </summary>
        public string? Id { get; set; }
        public string? ReceiverName { get; set; }
        public string? AddressName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AccountId { get; set; }
    }
}
