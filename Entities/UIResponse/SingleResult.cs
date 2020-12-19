
using System;
using System.Runtime.Serialization;

namespace Entities.UIResponse
{
    [DataContract(Name = "{0}Result")]
    public class SingleResult<T>
    {
        [DataMember]
        public int ResultCode { get; set; }

        [DataMember]
        public string ResultMessage { get; set; }

        [DataMember]
        public T Obj { get; set; }


        public static SingleResult<T> GetSuccessfulResult(T obj)
        {
            return new SingleResult<T>()
            {
                Obj = obj,
                ResultCode = 200,
                ResultMessage = "عملیات با موفقیت انجام شد"
            };
        }

        public static SingleResult<T> GetSuccessfulResult(T obj, string successMessage)
        {
            return new SingleResult<T>()
            {
                Obj = obj,
                ResultCode = 200,
                ResultMessage = successMessage
            };
        }

        public static SingleResult<T> GetFailResult(string message)
        {
           

            if (message == null)
            {
                return new SingleResult<T>()
                {

                    ResultCode = 400,
                    ResultMessage = "در انجام عملیات مشکلی پیش آمده است"
                };
            }
            else
            {
                return new SingleResult<T>()
                {

                    ResultCode = 400,
                    ResultMessage = message,
                };
            }
        }

        public static SingleResult<T> GetFailResult(string message,int code)
        {


            if (message == null)
            {
                return new SingleResult<T>()
                {

                    ResultCode = code,
                    ResultMessage = "در انجام عملیات مشکلی پیش آمده است"
                };
            }
            else
            {
                return new SingleResult<T>()
                {

                    ResultCode = code,
                    ResultMessage = message,
                };
            }
        }
    }
}