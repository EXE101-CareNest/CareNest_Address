﻿using CareNest_Address.Application.Features.Commands.Create;
using CareNest_Address.Application.Features.Commands.Update;

namespace CareNest_Address.Application.Exceptions.Validators
{
    public class Validate
    {
        /// <summary>
        /// kiểm tra toàn bộ tạo chi nhánh 
        /// </summary>
        /// <param name="command"></param>
        public static void ValidateCreate(CreateCommand command)
        {
            //ValidatePaymentMethod(command.PaymentMethod);
        }
        /// <summary>
        /// kiểm tra cập nhật chi nhánh 
        /// </summary>
        /// <param name="command"></param>
        public static void ValidateUpdate(UpdateCommand command)
        {
            //ValidatePaymentMethod(command.PaymentMethod);
        }
        /// <summary>
        /// Valid paymentMethod 
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="BadRequestException"></exception>
        //public static void ValidatePaymentMethod(string? paymentMethod)
        //{
        //    //-Không được để trống.
        //    if (string.IsNullOrWhiteSpace(paymentMethod))
        //    {
        //        throw new BadRequestException(MessageConstant.MissingPaymentMethod);
        //    }

        //}

    }
}
