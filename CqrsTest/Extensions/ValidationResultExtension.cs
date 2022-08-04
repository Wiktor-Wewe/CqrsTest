using CqrsTest;
using FluentValidation.Results;

namespace CqrsTest.Extensions
{
    public static class ValidationResultExtension
    {
        public static Result<T> ToResult<T>(this ValidationResult validationResult)
        {
            var result = new Result<T>
            {
                Code = ResultCode.BadRequest,
                Errors = validationResult.Errors.Select(v => new ErrorMessage()
                {
                    ProperyName = v.PropertyName,
                    Message = v.ErrorMessage
                })
            };
            return result;
        }
    }
}
