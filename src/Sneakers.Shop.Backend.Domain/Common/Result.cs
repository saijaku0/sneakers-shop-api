namespace Sneakers.Shop.Backend.Domain.Common
{
    public class Result : BaseResult
    {
        private Result(bool isSuccess, Error? error) 
            : base(isSuccess, error) { }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error);
    }

    public class Result<T> : BaseResult
    {
        public T? Value { get; }
        private Result(T value) : base(true, null)
        {
            Value = value;
        }
        private Result(Error error) : base(false, error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new (value);
        public static Result<T> Failure(Error error) => new (error);
    }
}
