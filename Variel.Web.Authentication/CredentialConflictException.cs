using System;

namespace Variel.Web.Authentication
{
    internal class CredentialConflictException : Exception
    {
        public CredentialConflictException()
        {
        }

        public CredentialConflictException(string message) : base(message)
        {
        }

        public CredentialConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}