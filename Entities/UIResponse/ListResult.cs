
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Entities.UIResponse
{
    [DataContract(Name = "{0}ResultList")]
    public class ListResult<T>
    {

        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string ResultMessage { get; set; }

        [DataMember] 
        public int? Totalcount { get; set; }

        [DataMember]
        public List<T> ObjList { get; set; }



        public static ListResult<T> GetSuccessfulResult(List<T> list)
        {
            return new ListResult<T>()
            {
                ObjList = list,
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد"
            };
        }

        public static ListResult<T> GetSuccessfulResult(List<T> list, string successMessage)
        {
            return new ListResult<T>()
            {
                ObjList = list,
                ResultCode = 200,
                ResultMessage = successMessage
            };
        }
        public static ListResult<T> GetSuccessfulResult(List<T> list, int totalcount)
        {
            return new ListResult<T>()
            {
                Totalcount = totalcount,
                ObjList = list,
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد"
            };
        }

        public static ListResult<T> GetFailResult(string message)
        {
            if (message == null)
            {
                return new ListResult<T>()
                {

                    ResultCode = 400,
                    ResultMessage = "در انجام عملیات مشکلی پیش آمده است"
                };
            }
            else
            {
                return new ListResult<T>()
                {

                    ResultCode = 400,
                    ResultMessage = message,
                };
            }


        }
    }
}