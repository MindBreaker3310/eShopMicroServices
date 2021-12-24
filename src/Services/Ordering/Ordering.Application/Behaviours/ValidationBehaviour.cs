using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        //這將會讀取所有Validator，只要有繼承AbstractValidator<TRequest>(來自FluentValidation)
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                //驗證輸入
                var context = new ValidationContext<TRequest>(request);
                //等待所有非同步執行結果 ( 對所有驗證項目進行非同步工作  )
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                //查看結果
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                //如果有錯的話
                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }
            //前處理的部份到此結束
            var response = await next();//Handler處理完回傳
            //後處理的部分從此開始
            return response;
        }
    }
}
