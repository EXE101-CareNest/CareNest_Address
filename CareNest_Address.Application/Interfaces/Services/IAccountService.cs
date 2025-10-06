using CareNest_Address.Application.Common;
using CareNest_Address.Application.DTOs;

namespace CareNest_Address.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ResponseResult<AccountResponse>> GetById(string? id);
    }
}
