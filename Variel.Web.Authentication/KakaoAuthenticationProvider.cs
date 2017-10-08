using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public class KakaoAuthenticationProvider<TAccount> : AuthenticationProvider<TAccount> where TAccount : class, IAccount
    {
        public KakaoAuthenticationProvider(AuthenticationDatabaseContext<TAccount> databaseContext)
            : base(databaseContext) { }

        public override async Task<TAccount> AuthenticateAsync(string id, string accessToken)
        {
            var credential = await Database.Credentials
                .Include(c => c.Account)
                .SingleOrDefaultAsync(c => c.Provider == AuthenticationProviders.Kakao && c.ProviderId == id);

            var kData = await GetKakaoUserDataAsync(accessToken);
            var remoteId = kData.Id;

            if (remoteId != id)
                return null;

            return credential?.Account;
        }

        public override async Task CreateAccountAsync(TAccount source, string id, string accessToken)
        {
            var credential = await Database.Credentials.FindAsync(AuthenticationProviders.Kakao, id);
            if (credential != null)
                throw new CredentialConflictException("이미 존재 하는 계정 정보입니다");

            credential = new Credential<TAccount>
            {
                Provider = AuthenticationProviders.Kakao,
                ProviderId = id
            };

            var kData = await GetKakaoUserDataAsync(accessToken);
            var remoteId = kData.Id;

            if (remoteId != id)
                throw new OAuthInfoNotMatchException("입력 된 아이디와 AccessToken을 이용해 얻어온 아이디가 다릅니다");
            
            credential.Account = source;

            Database.Accounts.Add(source);
            Database.Credentials.Add(credential);

            await Database.SaveChangesAsync();
        }

        public override bool Check(string id)
        {
            return Database.Credentials.Find(AuthenticationProviders.Kakao, id) == null;
        }


        private async Task<KakaoGetMeApiResponse> GetKakaoUserDataAsync(string accessToken)
        {
            const string apiHost = "kapi.kakao.com";
            const string apiVersion = "v1";

            var client = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
                }
            };

            var rawData = await client.GetStringAsync($"https://{apiHost}/{apiVersion}/user/me");
            var jsonData = JsonConvert.DeserializeObject<KakaoGetMeApiResponse>(rawData, new JsonSerializerSettings{ContractResolver = new DefaultContractResolver{NamingStrategy = new SnakeCaseNamingStrategy()}});
            return jsonData;
        }
        
        private class KakaoGetMeApiResponse
        {
            public string Id { get; set; }
            public string KaccountEmail { get; set; }
            public KakaoGetMeApiPropertiesResponse Properties { get; set; }
        }

        private class KakaoGetMeApiPropertiesResponse
        {
            public string Nickname { get; set; }
            public string ProfileImage { get; set; }
        }
    }
}
