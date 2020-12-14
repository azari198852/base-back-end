using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandCarftBaseServer.Tools
{
    public class SendSMS
    {
        public bool SendRegisterSMS(string mobileNo, string username, string pass)
        {

            var smsText = "نام کاربری و کلمه عبور شما در بازارچه اینترنتی صنایع دستی به شرح زیر می باشد:";
            smsText += "\\n";
            smsText += "نام کاربری: ";
            smsText += username;
            smsText += "\\n";
            smsText += "کلمه عبور: ";
            smsText += pass;
            smsText += "\\n";
            smsText += "لطفا به منظور ورود به فروشگاه ، به آدرس زیر مراجعه نمایید:";
            smsText += "\\n";
            smsText += "tabrizhandicrafts.com";

            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\"op\" : \"send\"" +
                                              ",\"uname\" : \"09144198583\"" +
                                              ",\"pass\":  \"1375989081\"" +
                                              ",\"message\" :" +
                                              $" \"{smsText}\"" +
                                              ",\"from\": \" +983000505\"" +
                                              $",\"to\" : [\"{mobileNo}\"]}}"
                    , ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.IsSuccessful;
        }

        public bool SendSuccessOrderPayment(long mobileNo, string orderNo, long payment)
        {

            var mob = "0" + mobileNo.ToString();

            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\"op\" : \"patternV2\"" +
                ",\"user\" : \"09144198583\"" +
                ",\"pass\":  \"1375989081\"" +
                ",\"fromNum\" : \"+983000505\"" +
               $",\"toNum\": \"{mob}\"" +
                ",\"patternCode\": \"veqt898ns1\"" +
                ",\"inputData\" : {\"order_reference\":" + $"\"{orderNo}\"" + ",\"payment\":" + $"\"{payment}\"" + ",\"total_paid\":" + $"\"{payment}\"" + "}}"
                , ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            return response.IsSuccessful;
        }

        public bool SendLoginSms(long mobileNo, int code)
        {

            var mob = "0" + mobileNo.ToString();

            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\"op\" : \"patternV2\"" +
                ",\"user\" : \"09144198583\"" +
                ",\"pass\":  \"1375989081\"" +
                ",\"fromNum\" : \"+983000505\"" +
               $",\"toNum\": \"{mob}\"" +
                ",\"patternCode\": \"6658ng6up6\"" +
                ",\"inputData\" : {\"verification-code\":" + $"\"{code}\"" + "}}"
                , ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            return response.IsSuccessful;
        }

        public string SendRestPassSms(long mobileNo, int code)
        {

            var smsText = "کد تایید شما برای تغییر رمز در سایت ";
            smsText += "tabrizhandicrafts.com";
            smsText += "\\n";
            smsText += "";
            smsText += code.ToString();
            smsText += " می باشد.";

            var mob = "0" + mobileNo.ToString();


            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\"op\" : \"patternV2\"" +
                ",\"user\" : \"09144198583\"" +
                ",\"pass\":  \"1375989081\"" +
                ",\"fromNum\" : \"+983000505\"" +
               $",\"toNum\": \"{mob}\"" +
                ",\"patternCode\": \"u6qmhwhh1h\"" +
                ",\"inputData\" : {\"verification-code\":" + $"\"{code}\"" + "}}"
                , ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);
            var a = "ErrorMessage:" + response.ErrorMessage + " Content: " + response.Content + " StatusCode:" +
                    response.StatusCode;
            return a;
           

        }

        public bool SendOrderSmsForSeller(long mobileNo)
        {

            var smsText = "سفارش جدید برای آماده سازی رسید ";
            smsText += "\\n";
            smsText += "tabrizhandicrafts.com";

            var mob = "0" + mobileNo.ToString();

            var client = new RestClient("http://188.0.240.110/api/select");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\"op\" : \"send\"" +
                                              ",\"uname\" : \"09144198583\"" +
                                              ",\"pass\":  \"1375989081\"" +
                                              ",\"message\" :" +
                                              $" \"{smsText}\"" +
                                              ",\"from\": \" +983000505\"" +
                                              $",\"to\" : [\"{mob}\"]}}"
                , ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response.IsSuccessful;
        }

    }
}
