using System.Threading.Tasks;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public abstract class AuthenticationProvider<TAccount> where TAccount : Account
    {
        protected AuthenticationDatabaseContext<TAccount> Database { get; }

        protected AuthenticationProvider(AuthenticationDatabaseContext<TAccount> database)
        {
            Database = database;
        }
        
        public abstract Task<TAccount> AuthenticateAsync(string id, string password);
        public abstract Task CreateAccountAsync(TAccount source, string id, string password);
        public abstract bool Check(string id);
    }
}
