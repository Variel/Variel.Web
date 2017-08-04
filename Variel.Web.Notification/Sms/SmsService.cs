using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Variel.Web.Common;

namespace Variel.Web.Notification.Sms
{
    public class SmsService
    {
        private readonly SettingsProvider _settings;

        public SmsService(SettingsProvider settings)
        {
            _settings = settings;
        }

        //these are variables for parameters
        string vUid = "userid=";
        string vKey = "&key=";
        string vReceiver = "&receiver=";
        string vSender = "&sender=";
        string vMessage = "&msg=";

        public async Task SendAsync(string phoneNum, string content)
        {
            if (String.IsNullOrWhiteSpace(phoneNum)
                || String.IsNullOrWhiteSpace(content))
                return;

            string sendSms = String.Concat("?",
                vUid, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:UserId")),
                vKey, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:Key")),
                vSender, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:Sender")),
                vReceiver, UrlEncoder.Default.Encode(phoneNum),
                vMessage, UrlEncoder.Default.Encode(content));

            using (var client = new HttpClient())
            {
                //configure baseaddress Aligo
                client.BaseAddress = new Uri("https://apis.aligo.in/");

                //send Request by GET method
                var response = await client.GetAsync(sendSms);

                //recieve JSON
                var stringResponse = await response.Content.ReadAsStringAsync();
                var aligoResponse = JsonConvert.DeserializeObject<AligoResponse>(stringResponse);

                if (aligoResponse.result_code != "1")
                {
                    throw new AligoException(aligoResponse.result_code, aligoResponse.message);
                }
            }

            //TODO: 로그 기록
        }

        public async Task SendAsync(string[] phoneNumbers, string content)
        {
            string phoneNums;
            if (phoneNumbers == null
                || phoneNumbers.Length == 0
                || String.IsNullOrWhiteSpace(phoneNums = String.Join(",", phoneNumbers))
                || String.IsNullOrWhiteSpace(content))
                return;

            string sendSmsMulti = String.Concat("?",
                vUid, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:UserId")),
                vKey, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:Key")),
                vSender, UrlEncoder.Default.Encode(_settings.GetStatic("AligoSettings:Sender")),
                vReceiver, UrlEncoder.Default.Encode(phoneNums),
                vMessage, UrlEncoder.Default.Encode(content));

            using (var client = new HttpClient())
            {
                //configure baseaddress Aligo
                client.BaseAddress = new Uri("https://apis.aligo.in/");

                //send Request by GET method
                var response = await client.GetAsync(sendSmsMulti);

                //recieve JSON
                var stringResponse = await response.Content.ReadAsStringAsync();
                var aligoResponse = JsonConvert.DeserializeObject<AligoResponse>(stringResponse);

                if (aligoResponse.result_code != "1")
                {
                    throw new AligoException(aligoResponse.result_code, aligoResponse.message);
                }
            }

            //TODO: 로그 기록
        }

        class AligoResponse
        {
            public string result_code { get; set; }
            public string message { get; set; }
            public string msg_id { get; set; }

            public string success_cnt { get; set; }
            public string error_cnt { get; set; }
        }

    }
}
