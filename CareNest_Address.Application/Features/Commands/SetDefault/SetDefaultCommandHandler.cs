using CareNest_Address.Application.Exceptions;
using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Commons.Constant;
using CareNest_Address.Domain.Entitites;

namespace CareNest_Address.Application.Features.Commands.SetDefault
{
    public class SetDefaultCommandHandler : ICommandHandler<SetDefaultCommand, Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _service;

        public SetDefaultCommandHandler(IUnitOfWork unitOfWork, IAccountService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public async Task<Address> HandleAsync(SetDefaultCommand command)
        {
            // validate account id via account service (if provided)
            if (string.IsNullOrEmpty(command.AccountId))
            {
                // try to read account from address
                Address? existing = await _unitOfWork.GetRepository<Address>().GetByIdAsync(command.Id);
                if (existing == null)
                    throw new BadRequestException("Id: " + MessageConstant.NotFound);
                command.AccountId = existing.AccountId;
            }

            var account = await _service.GetById(command.AccountId);
            if (account == null || account.Data == null || account.Data.Data == null)
                throw new BadRequestException("Account: " + MessageConstant.NotFound);

            // find target address
            Address? address = await _unitOfWork.GetRepository<Address>().GetByIdAsync(command.Id)
                ?? throw new BadRequestException("Id: " + MessageConstant.NotFound);

            // if setting this address as default, clear others for same account
            if (command.IsDefault)
            {
                var repo = _unitOfWork.GetRepository<Address>();
                var others = await repo.FindAsync(a => a.AccountId == command.AccountId && a.Id != command.Id && (a.IsDefault == true));
                foreach (var o in others)
                {
                    o.IsDefault = false;
                    repo.Update(o);
                }

                address.IsDefault = true;
                repo.Update(address);
                await _unitOfWork.SaveAsync();
                return address;
            }

            // If unsetting default, just update the single address
            address.IsDefault = false;
            _unitOfWork.GetRepository<Address>().Update(address);
            await _unitOfWork.SaveAsync();
            return address;
        }
    }
}
