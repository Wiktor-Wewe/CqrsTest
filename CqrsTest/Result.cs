using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CqrsTest
{
    public class Result
    {
        public bool Success => Code == ResultCode.Ok;

        public IEnumerable<ErrorMessage> Errors { get; set; }

        [JsonIgnore]
        public ResultCode Code { get; set; }

        public static Result Ok()
        {
            return new Result()
            {
                Code = ResultCode.Ok
            };
        }

        public static Result<T> Ok<T>(T Value)
        {
            return new Result<T>
            {
                Value = Value,
                Code = ResultCode.Ok
            };
        }

        public static Result BadRequest(string message)
        {
            var result = new Result
            {
                Code = ResultCode.BadRequest,
                Errors = new List<ErrorMessage>()
                {
                    new ErrorMessage()
                    {
                        Message = message
                    }
                }
            };
            return result;
        }

        public static Result<T> BadRequest<T>(IEnumerable<ErrorMessage> messages)
        {
            var result = new Result<T>
            {
                Code = ResultCode.BadRequest,
                Errors = messages
            };

            return result;
        }

        public static Result<T> NotFound<T>(Guid id)
        {
            var result = new Result<T>
            {
                Code = ResultCode.NotFound,
                Errors = new List<ErrorMessage>()
                {
                    new ErrorMessage()
                    {
                        Message = $"Nie ma obiektu o id {id}"
                    }
                }
            };

            return result;
        }

        public static Result NotFound(Guid id)
        {
            var result = new Result
            {
                Code = ResultCode.NotFound,
                Errors = new List<ErrorMessage>()
                {
                    new ErrorMessage()
                    {
                        Message = $"Nie ma obiektu o id {id}"
                    }
                }
            };

            return result;
        }

    }
    public class Result<T> : Result
    {
        public T Value { get; set; }
    }

    public class ErrorMessage
    {
        public string ProperyName { get; set; }
        public string Message { get; set; }
    }

    public enum ResultCode
    {
        Ok,
        BadRequest,
        NotFound
    }
}
