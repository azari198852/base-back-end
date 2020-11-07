
using System;
using System.Collections.Generic;

namespace Entities.UIResponse
{
    public class LongResult : MasterResult<long>
    {
        public static LongResult GetSuccessResult(List<long> list, int totalCount)
        {
            return new LongResult()
            {
                ObjList = list,
                TotalCount = totalCount,
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد"
            };
        }
        public static LongResult GetSuccessResult(List<long> list, int totalCount, string successMessage)
        {
            return new LongResult()
            {
                ObjList = list,
                TotalCount = totalCount,
                ResultCode = 200,
                ResultMessage = successMessage
            };
        }
        public static LongResult GetSingleSuccessfulResult(long obj)
        {
            return new LongResult()
            {
                ObjList = new List<long>() { obj },
                TotalCount = 0,
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد"
            };
        }

        public static LongResult GetSingleSuccessfulResult(long obj, string successMessage)
        {
            return new LongResult()
            {
                ObjList = new List<long>() { obj },
                TotalCount = 0,
                ResultCode = 200,
                ResultMessage = successMessage
            };
        }

        public static LongResult GetFailResult(string message)
        {

            if (message == null)
            {
                return new LongResult()
                {

                    ResultCode = 400,
                    ResultMessage = "در انجام عملیات مشکلی پیش آمده است"
                };
            }
            else
            {
                return new LongResult()
                {

                    ResultCode = 400,
                    ResultMessage = message,
                };
            }


        }
    }
}