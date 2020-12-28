using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class BusinessException : Exception
    {
        public int Code { set; get; }

        public BusinessException(string message, int code) : base(message)
        {
            Code = code;
        }

        public BusinessException(XError error) : base(error.Message)
        {
            Code = error.Code;
        }

        public static BusinessException TryParse(Exception ex)
        {
            if (ex.GetType() == typeof(BusinessException))
                return (BusinessException)ex;
            else
                return new BusinessException(XError.GeneralError);
        }


        public override string ToString()
        {
            return string.Format("ErrorCode: {0} - ErrorMessage: {1}", Code, Message);
        }
    }
}
