
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class XError
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public XError() { }
        public XError(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static XError GeneralError => new XError(1, "خطا در سامانه");

        public class AuthenticationErrors
        { // starts from 2000
            public static XError UserNameOrPasswordIsWrong() => new XError(2001, "خطا در احراز هویت کاربر");
            public static XError NeedLoginAgain() => new XError(2002, "لطفا جهت ادامه کار مجددا وارد شوید");
            public static XError NotHaveRequestedRole() => new XError(2003, "نقش انتخابی جزو نقش های شما نمی باشد");
        }

        public class GetDataErrors
        { // starts from 3000
            public static XError NotFound() => new XError(3000, "رکورد مورد نظر یافت نشد");
            public static XError FileNotFound() => new XError(3001, "،فایل مورد نظر یافت نشد");

        }

        public class BusinessErrors
        { // starts from 4000
            public static XError FileMaxSizeReach() => new XError(4000, "حجم فایل بیش از مقدار مجاز می باشد.");
            public static XError UserAlreadyExists() => new XError(4002, "کاربر با مشخصات وارد شده در سامانه موجود می باشد");
            public static XError InvalidOrder() => new XError(4003, "سفارش وجود ندارد با سفارش مربوط به کاربر جاری نمی باشد");
            public static XError PaymentInfoNotFound() => new XError(4004, "اطلاعاتی برای پارامترهای ارسالی یافت نشد");
            public static XError FailedPayment() => new XError(4005, "پرداخت نا موفق");

        }

        public class IncomingSerivceErrors
        { // starts from 5000
            public static XError PostSeerivcError() => new XError(5000, "خطا در دریافت اطلاعات پستی");


        }
    }
}
