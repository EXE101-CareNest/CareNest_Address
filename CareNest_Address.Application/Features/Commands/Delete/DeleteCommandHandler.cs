using CareNest_Address.Application.Exceptions;
using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Commands.Delete
{
    public class DeleteCommandHandler : ICommandHandler<DeleteCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(DeleteCommand command)
        {
            // Lấy address theo ID
            Address? address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(command.Id)
                                              ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            _unitOfWork.GetRepository<Address>().Delete(address);

            await _unitOfWork.SaveAsync();
        }
    }
}
