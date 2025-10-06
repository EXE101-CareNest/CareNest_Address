namespace CareNest_Address.Application.DTOs
{
    public class AccountResponse
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public bool? IsActive { get; set; }

    }
}
