namespace Sneakers.Shop.Backend.Domain.Common
{
    public abstract class BaseResult
    {
        public Error? Error { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        protected BaseResult(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
