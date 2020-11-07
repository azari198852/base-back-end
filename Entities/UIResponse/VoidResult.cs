
using System;

namespace Entities.UIResponse
{
    public class VoidResult
    {
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }

        // ------------------------------------------------------------ Sucsess
        public static VoidResult GetSuccessResult()
        {
            return new VoidResult()
            {
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد",
            };
        }
        public static VoidResult GetSuccessResult(string message)
        {
            return new VoidResult()
            {
                ResultCode = 200,
                ResultMessage = message
            };
        }

        // ------------------------------------------------------------ Error

        public static VoidResult GetFailResult(string message)
        {

            if (message == null)
            {
                return new VoidResult()
                {

                    ResultCode = 400,
                    ResultMessage = "در انجام عملیات مشکلی پیش آمده است"
                };
            }
            else
            {
                return new VoidResult()
                {

                    ResultCode = 400,
                    ResultMessage = message,
                };
            }
        }
    }
}