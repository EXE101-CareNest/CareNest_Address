using CareNest_Address.Application.Common;
using CareNest_Address.Application.Interfaces.CQRS.Queries;


namespace CareNest_Address.Application.Features.Queries.GetAllPaging
{
    public class GetAllPagingQuery : IQuery<PageResult<AddressResponse>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; } // "Name", "Note", "CreatedAt"
        public string? SortDirection { get; set; } // "asc" or "desc"
        public string? AccountId { get; set; } // "asc" or "desc"
    }
}
