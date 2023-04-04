using System.Collections.Generic;

namespace ServiceHooks.Common
{
    public class OperationResultBase 
    {
        public bool Succeded { get; protected set; }

        public string[] Errors { get; set; }
    }

    public class OperationResult : OperationResultBase
    {
        public OperationResult()
        {
        }

        public static OperationResult Ok()
        {
            return new OperationResult
            {
                Succeded = true
            };
        }

        public static OperationResult Fail(params string[] errors)
        {
            return new OperationResult
            {
                Succeded = false,
                Errors = errors
            };
        }
    }

    public class OperationResult<T> : OperationResultBase
    {
        public T Result { get; set; }
        public static OperationResult<T> Ok(T result)
        {
            return new OperationResult<T>
            {
                Result = result,
                Succeded = true,
            };
        }

        public static OperationResult<T> Fail(params string[] errors)
        {
            return new OperationResult<T>
            {
                Result = default(T),
                Succeded = false,
                Errors = errors
            };
        }
    }
}
