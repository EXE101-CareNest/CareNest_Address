using CareNest_Address.Application.Common;
using CareNest_Address.Application.DTOs;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Address.Domain.Commons.Base;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Infrastructure.ApiEndpoints;

namespace CareNest_Address.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAPIService _apiService;

        public AccountService(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<ResponseResult<AccountResponse>> GetById(string? id)
        {
            var shop = await _apiService.GetAsync<AccountResponse>("account", AccountEndpoint.GetById(id));
            if (!shop.IsSuccess)
            {
                throw BaseException.BadRequestBadRequestResponse("Account Id : " + MessageConstant.NotFound);
            }
            return shop;
        }
    }
}
