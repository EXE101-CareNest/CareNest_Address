using CareNest_Address.Application.Common;
using CareNest_Address.Application.Interfaces.CQRS.Queries;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQueryHandler : IQueryHandler<GetAllPagingQuery, PageResult<AddressResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPagingQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageResult<AddressResponse>> HandleAsync(GetAllPagingQuery query)
        {
            var selector = ObjectMapperExtensions.CreateMapExpression<Address, AddressResponse>();

            var orderByFunc = GetOrderByFunc(query.SortColumn, query.SortDirection);
            // Thêm điều kiện lọc theo ShopId nếu có
            System.Linq.Expressions.Expression<Func<Address, bool>>? predicate = null;
            if (!string.IsNullOrWhiteSpace(query.AccountId))
            {
                predicate = s => s.AccountId == query.AccountId;
            }

            IEnumerable<AddressResponse> a = await _unitOfWork.GetRepository<Address>().FindAsync(
                predicate: predicate,
                orderBy: orderByFunc,
                selector: selector,
                pageSize: query.PageSize,
                pageIndex: query.Index);

            return new PageResult<AddressResponse>(a, 1, query.Index, query.PageSize);
        }


        private Func<IQueryable<Address>, IOrderedQueryable<Address>> GetOrderByFunc(string? sortColumn, string? sortDirection)
        {
            var ascending = string.IsNullOrWhiteSpace(sortDirection) || sortDirection.ToLower() != "desc";

            return sortColumn?.ToLower() switch
            {
                "updateat" => q => ascending ? q.OrderBy(a => a.UpdatedAt) : q.OrderByDescending(a => a.UpdatedAt),
                _ => q => q.OrderBy(a => a.CreatedAt) // fallback nếu không có sortColumn
            };
        }
    }
}
