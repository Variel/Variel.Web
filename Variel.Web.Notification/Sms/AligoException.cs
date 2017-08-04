using System;
using System.Collections.Generic;
using System.Text;

namespace Variel.Web.Notification.Sms
{
    public class AligoException : System.Exception
    {
        public AligoException() { }
        public AligoException(string resultCode, string message) : base(message)
        {
            ResultCode = resultCode;
        }
        public string ResultCode { get; }
    }
}
