using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Variel.Web.Common;
using Variel.Web.Extensions;

namespace Variel.Web.Session
{
    public class SessionService<TAccount> where TAccount : class, IAccount
    {
        private const string SessionKey = "AccountId";

        private readonly HttpContext _context;
        private readonly IAccountDatabaseContext<TAccount> _database;

        public SessionService(IHttpContextAccessor contextAccessor, IAccountDatabaseContext<TAccount> database)
        {
            _database = database;
            _context = contextAccessor.HttpContext;
        }

        public async Task<TAccount> GetUserAsync()
        {
            var user = _context.Items["User"] as TAccount;
            if (user != null)
                return user;

            await _context.Session.LoadAsync();
            var id = _context.Session.GetInt64(SessionKey);

            if (id == null)
                return null;

            user = await _database.Accounts.FindAsync(id.Value);
            _context.Items["User"] = user;

            return user;
        }

        public async Task LoginAsync(TAccount user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Session.SetInt64(SessionKey, user.Id);
            _context.Items["User"] = user;

            await _database.SaveChangesAsync();
        }

        public void Logout()
        {
            _context.Session.Remove(SessionKey);
            _context.Items.Remove("User");
        }

        public bool IsLoggedIn => GetUserAsync().Result != null;
    }
}
