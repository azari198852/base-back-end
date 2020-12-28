using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandCarftBaseServer.Tools

{
    public class General
    {


        public class Results_
        {
            public static int SuccessCode() { return 0; }
            public static string SuccessMessage() { return "عملیات با موفقیت انجام شد"; }
            public static string ErrorMessage() { return "خطا در سامانه"; }
            public static int SingleResultTotalCount() { return -1; }
            public static int FailResultTotalCount() { return -2; }
            public static int FieldNullErrorCode() { return 111; }
            public static int FieldNotValidErrorCode() { return 110; }
        }

        public class Regexes_
        {
            public static string NumericStringRegex()
            {
                return @"\d+";
            }
            public static string NumericStringRegex(int length)
            {
                return @"^(\d{" + length + @"})$";
            }
            public static string FloatNumericStringRegex(int length)
            {
                return @"^(\d{" + length + @"})(?:.\d{0,1})$";
            }
            public static string NumericStringRegex(int minLength, int maxLength)
            {
                return @"^\d{" + minLength + @"," + maxLength + @"}$";
            }
            public static string AlphabetStringRegex()
            {
                return @"\w+";
            }
            public static string AlphabetStringRegex(int length)
            {
                return @"^\w{" + length + @"}$";
            }
            public static string AlphabetStringRegex(int minLength, int maxLength)
            {
                return @"^\w{" + minLength + @"," + maxLength + @"}$";
            }
            public static string AlphabetAndNumericStringRegex()
            {
                return @"[a-zA-Z0-9]+";
            }
            public static string AlphabetAndNumericStringRegex(int length)
            {
                return @"^[a-zA-Z0-9]{" + length + @"}$";
            }
            public static string AlphabetAndNumericStringRegex(int minLength, int maxLength)
            {
                return @"^[a-zA-Z0-9]{" + minLength + @"," + maxLength + @"}$";
            }
            public static string AlphabetAndNumericAndStarStringRegex()
            {
                return @"[a-zA-Z0-9/*]+";
            }
            public static string AlphabetAndNumericAndStarStringRegex(int length)
            {
                return @"^[a-zA-Z0-9/*]{" + length + @"}$";
            }
            public static string AlphabetAndNumericAndStarRegex(int minLength, int maxLength)
            {
                return @"^[a-zA-Z0-9/*]{" + minLength + @"," + maxLength + @"}$";
            }
            public static string ShamsiDateRegex()
            {
                return @"^[1][2-4][0-9][0-9]\/((01|02|03|04|05|06)\/(0[1-9]|[1-2][0-9]|3[0-1]))|((07|08|09|10|11|12)\/(0[1-9]|[1-2][0-9]|30))$";
            }

        }

        public class Messages_
        {

            public class NullInputMessages_
            {
                public static string GeneralNullMessage(string fieldName)
                {
                    return string.Format("فیلد {0} خالی است", fieldName);
                }
            }

            public class NotValidMessages_
            {
                public static string FieldIsNotValidMessage(string fieldName)
                {
                    return string.Format("فیلد {0} نامعتبر است", fieldName);
                }

                public static string NotValidMobileNumber = "شماره موبایل به درستی ثبت نشده است";
            }
        }

      
    }
}
