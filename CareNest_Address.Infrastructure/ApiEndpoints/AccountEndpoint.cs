namespace CareNest_Address.Infrastructure.ApiEndpoints
{
    public class AccountEndpoint
    {
        public static string GetById(string? id) => $"/api/accounts/{id}";
    }
}