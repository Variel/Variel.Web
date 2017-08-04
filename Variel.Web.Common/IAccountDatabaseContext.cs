using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Variel.Web.Common
{
    public interface IAccountDatabaseContext<TAccount> where TAccount : Account
    {
        DbSet<TAccount> Accounts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        int SaveChanges();
    }
}
