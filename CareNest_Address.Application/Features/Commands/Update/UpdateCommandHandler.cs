using CareNest_Address.Application.Exceptions;
using CareNest_Address.Application.Exceptions.Validators;
using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Domain.Entitites;
using Shared.Helper;

namespace CareNest_Address.Application.Features.Commands.Update
{
    public class UpdateCommandHandler : ICommandHandler<UpdateCommand, Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _service;

        public UpdateCommandHandler(IUnitOfWork unitOfWork, IAccountService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<Address> HandleAsync(UpdateCommand command)
        {
            // Gọi validator để kiểm tra dữ liệu
            //Validate.ValidateUpdate(command);
            var account = await _service.GetById(command.AccountId);

            // Tìm để cập nhật
            Address? address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(command.Id)
               ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            address.PhoneNumber = command.PhoneNumber ?? address.PhoneNumber;
            address.AddressName = command.AddressName ?? address.AddressName;
            address.AccountId = account.Data!.Data!.Id;
            address.ReceiverName = command.ReceiverName ?? address.ReceiverName;
            address.UpdatedAt = TimeHelper.GetUtcNow();

            _unitOfWork.GetRepository<Address>().Update(address);
            await _unitOfWork.SaveAsync();
            return address;

        }
    }
}
