using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public class FacebookAuthenticationProvider<TAccount> : AuthenticationProvider<TAccount> where TAccount : class, IAccount
    {
        public FacebookAuthenticationProvider(AuthenticationDatabaseContext<TAccount> databaseContext)
            : base(databaseContext) { }

        public override async Task<TAccount> AuthenticateAsync(string id, string accessToken)
        {
            var credential = await Database.Credentials
                .Include(c => c.Account)
                .SingleOrDefaultAsync(c => c.Provider == AuthenticationProviders.Self && c.ProviderId == id);

            var fbData = await GetFacebookUserDataAsync(accessToken);
            var remoteId = fbData.Id;

            if (remoteId != id)
                return null;

            return credential?.Account;
        }

        public override async Task CreateAccountAsync(TAccount source, string id, string accessToken)
        {
            var credential = await Database.Credentials.FindAsync(AuthenticationProviders.Self, id);
            if (credential != null)
                throw new CredentialConflictException("이미 존재 하는 계정 정보입니다");

            credential = new Credential<TAccount>
            {
                Provider = AuthenticationProviders.Facebook,
                ProviderId = id
            };

            var fbData = await GetFacebookUserDataAsync(accessToken);
            var remoteId = fbData.Id;

            if (remoteId != id)
                throw new OAuthInfoNotMatchException("입력 된 아이디와 AccessToken을 이용해 얻어온 아이디가 다릅니다");
            
            credential.Account = source;

            Database.Accounts.Add(source);
            Database.Credentials.Add(credential);
            await Database.SaveChangesAsync();
        }

        public override bool Check(string id)
        {
            return Database.Credentials.Find(AuthenticationProviders.Facebook, id) == null;
        }


        private async Task<FacebookGetMeApiResponse> GetFacebookUserDataAsync(string accessToken)
        {
            const string graphApiHost = "graph.facebook.com";
            const string graphApiVersion = "v2.5";

            var client = new HttpClient();
            var rawData = await client.GetStringAsync($"https://{graphApiHost}/{graphApiVersion}/me?fields=id,name&access_token={accessToken}");
            var jsonData = JsonConvert.DeserializeObject<FacebookGetMeApiResponse>(rawData);

            return jsonData;
        }
        
        private class FacebookGetMeApiResponse
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
