using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ResultLibrary
{
    public enum ResultState
    {
        Success,
        NotFound,
        BadData
    }
    public class Result<T>
    {
        public ResultState State { get; protected set; }
        public string ErrorMessage { get; protected set; }
        public T Value { get; protected set; }

        public Result(T value):this(ResultState.Success)
        {
            Value = value;
        }
        public Result(string erroMessage,ResultState state)
        {
            State = state;
            ErrorMessage = erroMessage;
        }
        protected Result(ResultState state)
        {
            State = state;
        }

        public static explicit operator T(Result<T> result)
        {
           return result.Value;
        }

        public static implicit operator Result<T>(T result)
        {
            return new Result<T>(result);
        }
    }
}
