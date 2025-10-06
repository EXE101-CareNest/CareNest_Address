
using CareNest_Address.Application.Common;
using CareNest_Address.Application.Features.Commands.Create;
using CareNest_Address.Application.Features.Commands.Delete;
using CareNest_Address.Application.Features.Commands.Update;
using CareNest_Address.Application.Features.Queries.GetAllPaging;
using CareNest_Address.Application.Features.Queries.GetById;
using CareNest_Address.Application.Interfaces.CQRS;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Domain.Entitites;
using CareNest_Address.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace CareNest_Address.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class addressController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public addressController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Hiển thị toàn bộ danh sách chi nhánh  hiện có trong hệ thống với phân trang và sắp xếp
        /// </summary>
        /// <param name="pageIndex">trang hiện tại</param>
        /// <param name="pageSize">Số lượng phần tử trong trang</param>
        /// <param name="sortColumn">cột muốn sort: name, updateat,ownerid</param>
        /// <param name="sortDirection">cách sort asc or desc</param>
        /// <returns>Danh sách chi nhánh </returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? accountId = null)
        {
            var query = new GetAllPagingQuery()
            {
                Index = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                AccountId = accountId
            };
            var result = await _dispatcher.DispatchQueryAsync<GetAllPagingQuery, PageResult<AddressResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Hiển thị chi tiết chi nhánh  theo id
        /// </summary>
        /// <param name="id">Id chi nhánh </param>
        /// <returns>chi tiết chi nhánh </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdQuery() { Id = id };
            Address result = await _dispatcher.DispatchQueryAsync<GetByIdQuery, Address>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// tạo mới chi nhánh 
        /// </summary>
        /// <param name="command">thông tin chi nhánh </param>
        /// <returns>thông tin chi nhánh  mới tạo xog</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            Address result = await _dispatcher.DispatchAsync<CreateCommand, Address>(command);

            return this.OkResponse(result, MessageConstant.SuccessCreate);
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh 
        /// </summary>
        /// <param name="id">Id chi nhánh </param>
        /// <param name="request">các thông tin cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRequest request)
        {

            var command = new UpdateCommand()
            {
                Id = id,
                PhoneNumber = request.PhoneNumber,
                AccountId= request.AccountId,
                AddressName = request.AddressName,
                ReceiverName = request.ReceiverName
            };
            Address result = await _dispatcher.DispatchAsync<UpdateCommand, Address>(command);

            return this.OkResponse(result, MessageConstant.SuccessUpdate);
        }

        /// <summary>
        /// xoá chi nhánh 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dispatcher.DispatchAsync(new DeleteCommand { Id = id });
            return this.OkResponse(MessageConstant.SuccessDelete);
        }
    }
}
