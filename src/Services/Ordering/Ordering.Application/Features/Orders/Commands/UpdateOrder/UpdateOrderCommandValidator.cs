using System;
using FluentValidation;
namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            //p代表property
            RuleFor(p => p.UserName)//驗證UserName   
                .NotEmpty().WithMessage("UserName 必填")//不可為空值，若為空值回傳訊息
                .NotNull()
                .MaximumLength(50).WithMessage("UserName 字數過長");

            RuleFor(p => p.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress 必填");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("TotalPrice 必填")
                .GreaterThan(0).WithMessage("TotalPrice 需要大於0");
        }
    }
}
