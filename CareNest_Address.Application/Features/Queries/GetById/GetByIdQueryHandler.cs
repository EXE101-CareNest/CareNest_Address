using CareNest_Address.Application.Interfaces.CQRS.Queries;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Queries.GetById
{
    public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, Address>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Address> HandleAsync(GetByIdQuery query)
        {
            Address? address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(query.Id);

            if (address == null)
            {
                throw new Exception(MessageConstant.NotFound);
            }
            return address;
        }
    }
}
