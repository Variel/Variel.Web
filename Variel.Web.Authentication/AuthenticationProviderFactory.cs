using System;
using System.Collections.Generic;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public class AuthenticationProviderFactory<TAccount> : IAuthenticationProviderFactory<TAccount> where TAccount : Account
    {
        static readonly Dictionary<AuthenticationProviders, Type> Providers = new Dictionary<AuthenticationProviders, Type>
        {
            [AuthenticationProviders.Self] = typeof(SelfAuthenticationProvider<TAccount>),
            [AuthenticationProviders.Facebook] = typeof(FacebookAuthenticationProvider<TAccount>)
        };

        
        private readonly AuthenticationDatabaseContext<TAccount> _database;

        public AuthenticationProviderFactory(AuthenticationDatabaseContext<TAccount> database)
        {
            this._database = database;
        }

        public AuthenticationProvider<TAccount> Create(AuthenticationProviders provider)
        {
            return Activator.CreateInstance(Providers[provider], _database) as AuthenticationProvider<TAccount>;
        }
    }
}
