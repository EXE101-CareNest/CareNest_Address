using CareNest_Address.Application.Exceptions.Validators;
using CareNest_Address.Application.Interfaces.CQRS.Commands;
using CareNest_Address.Application.Interfaces.Services;
using CareNest_Address.Application.Interfaces.UOW;
using CareNest_Address.Domain.Entitites;
using Shared.Helper;

namespace CareNest_Address.Application.Features.Commands.Create
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _service;


        public CreateCommandHandler(IUnitOfWork unitOfWork, IAccountService shopService)
        {
            _service = shopService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Address> HandleAsync(CreateCommand command)
        {
            //Validate.ValidateCreate(command);
            var account = await _service.GetById(command.AccountId);
            Address address = new()
            {
                PhoneNumber = command.PhoneNumber,
                AccountId = account.Data.Data.Id,
                ReceiverName = command.ReceiverName,
                IsDefault = false,
                AddressName = command.AddressName,
                CreatedAt = TimeHelper.GetUtcNow()
            };
            await _unitOfWork.GetRepository<Address>().AddAsync(address);
            await _unitOfWork.SaveAsync();

            return address;
        }
    }
}
