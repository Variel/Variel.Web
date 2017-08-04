using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public interface IAuthenticationProviderFactory<TAccount> where TAccount : class, IAccount
    {
        AuthenticationProvider<TAccount> Create(AuthenticationProviders provider);
    }
}
