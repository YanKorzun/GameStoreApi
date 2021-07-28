using GameStore.BL.Enums;

namespace GameStore.BL.ResultWrappers
{
    public class ServiceResult
    {
        protected ServiceResult()
        {
        }

        public ServiceResult(ServiceResultType result)
        {
            Result = result;
        }

        public ServiceResult(ServiceResultType result, string message)
        {
            Result = result;
            ErrorMessage = message;
        }

        public ServiceResultType Result { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult()
        {
        }

        public ServiceResult(ServiceResultType result)
        {
            Result = result;
        }

        public ServiceResult(ServiceResultType result, string message)
        {
            Result = result;
            ErrorMessage = message;
        }

        public ServiceResult(ServiceResultType result, T data)
        {
            Result = result;
            Data = data;
        }

        public ServiceResult(ServiceResultType result, string message, T data)
        {
            Result = result;
            ErrorMessage = message;
            Data = data;
        }

        public T Data { get; set; }
    }
}