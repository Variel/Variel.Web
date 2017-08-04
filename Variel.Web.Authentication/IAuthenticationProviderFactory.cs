using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public interface IAuthenticationProviderFactory<TAccount> where TAccount : Account
    {
        AuthenticationProvider<TAccount> Create(AuthenticationProviders provider);
    }
}
