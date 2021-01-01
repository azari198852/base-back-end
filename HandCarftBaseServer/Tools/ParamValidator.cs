using Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandCarftBaseServer.Tools

{
    public class ParamValidator
    {
        private List<string> errorList { get; set; }

        public ParamValidator()
        {
            errorList = new List<string>();
        }

        public ParamValidator Add(string message)
        {
            if (!string.IsNullOrEmpty(message))
                errorList.Add(message);
            return this;
        }

        public ParamValidator Clear()
        {
            errorList = new List<string>();
            return this;
        }

        public ParamValidator Throw(int errorCode)
        {
            if (errorList.Count != 0)
            {
                throw new BusinessException(new XError(errorCode, string.Join(Environment.NewLine, errorList)));
            }
            else
                return this;
        }

 
        #region General Validations
        public ParamValidator ValidateRegex(string obj, string regex, string message)
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj)) return this;
            if (!Regex.IsMatch(obj, regex)) errorList.Add(message);

            return this;
        }

        public ParamValidator ValidateLength(string obj, int minLength, int maxLength, string message)
        {
            if (obj.Length < minLength || obj.Length > maxLength)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateLength(long obj, long min, long max, string message)
        {
            if (obj < min || obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateLength(IList objList, long min, long max, string message)
        {
            if (objList.Count < min || objList.Count > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateRange(DateTime obj, DateTime min, DateTime max, string message)
        {
            if (obj < min || obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateMaxRange(DateTime obj, DateTime max, string message)
        {
            if (obj > max)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateMinRange(DateTime obj, DateTime min, string message)
        {
            if (obj < min)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateEmail(string obj, string message = "فیلد ایمیل نامعتبر است")
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj))
                return this;
            try
            {
                System.Net.Mail.MailAddress mail = new System.Net.Mail.MailAddress(obj);
            }
            catch (Exception)
            {
                errorList.Add(message);
            }
            return this;
        }

        public ParamValidator ValidateMobile(long obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj < 9000000000 || obj >= 10000000000)
            {
                errorList.Add(message);
            }
            return this;
        }

        public ParamValidator ValidateMobileWithZero(string obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj.Length != 11)
            {
                errorList.Add(message);
            }
            if (!obj.Substring(0, 2).Equals("09"))
            {
                errorList.Add(message);
            }
            return this;
        }
        public ParamValidator ValidateMobileWithOutZero(string obj, string message = "فیلد شماره موبایل نامعتبر است")
        {
            if (obj.Length != 10)
            {
                errorList.Add(message);
            }
            if (!obj.Substring(0, 1).Equals("9"))
            {
                errorList.Add(message);
            }
            return this;
        }
        #endregion


        #region Null Validations

        public ParamValidator ValidateNull(int? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(long? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(double? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(byte? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(DateTime? obj, string message)
        {
            if (!obj.HasValue)
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(string obj, string message)
        {
            if (string.IsNullOrEmpty(obj) || string.IsNullOrWhiteSpace(obj))
                errorList.Add(message);
            return this;
        }

        public ParamValidator ValidateNull(IList objList, string message)
        {
            if (objList == null || objList.Count == 0)
                errorList.Add(message);
            return this;
        }

        #endregion


    }
}
