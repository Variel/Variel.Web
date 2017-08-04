using System;

namespace Variel.Web.Authentication
{
    internal class OAuthInfoNotMatchException : Exception
    {
        public OAuthInfoNotMatchException()
        {
        }

        public OAuthInfoNotMatchException(string message) : base(message)
        {
        }

        public OAuthInfoNotMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}