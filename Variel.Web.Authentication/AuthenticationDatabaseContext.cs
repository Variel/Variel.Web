using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Variel.Web.Common;

namespace Variel.Web.Authentication
{
    public class AuthenticationDatabaseContext<TAccount> : DbContext, IAccountDatabaseContext<TAccount> where TAccount : class, IAccount
    {
        public AuthenticationDatabaseContext() { }
        public AuthenticationDatabaseContext(DbContextOptions options) : base(options) {}


        public DbSet<TAccount> Accounts { get; set; }
        public DbSet<Credential<TAccount>> Credentials { get; set; }
    }
}
