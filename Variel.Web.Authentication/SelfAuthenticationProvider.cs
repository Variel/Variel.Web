using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Variel.Web.Authentication;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public class SelfAuthenticationProvider<TAccount> : AuthenticationProvider<TAccount> where TAccount : class, IAccount
    {
        public SelfAuthenticationProvider(AuthenticationDatabaseContext<TAccount> database)
            : base(database) { }
        
        public override async Task<TAccount> AuthenticateAsync(string id, string password)
        {
            var credential = await Database.Credentials
                .Include(c => c.Account)
                .SingleOrDefaultAsync(c => c.Provider == AuthenticationProviders.Self && c.ProviderId == id);

            if (credential?.VerifyPassword(password) == true)
                return credential.Account;

            return null;
        }

        public override async Task CreateAccountAsync(TAccount source, string id, string password)
        {
            var credential = await Database.Credentials.FindAsync(AuthenticationProviders.Self, id);
            if (credential != null)
                throw new CredentialConflictException("이미 존재 하는 계정 정보입니다");
            
            credential = new Credential<TAccount>
            {
                Provider = AuthenticationProviders.Self,
                ProviderId = id
            };
            credential.SetPassword(password);
            
            credential.Account = source;

            Database.Accounts.Add(source);
            Database.Credentials.Add(credential);

            await Database.SaveChangesAsync();
        }

        public override bool Check(string id)
        {
            return Database.Credentials.Find(AuthenticationProviders.Self, id) == null;
        }
    }
}
