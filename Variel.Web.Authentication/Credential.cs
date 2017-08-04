using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Variel.Web.Common;
using Variel.Web.Helpers;

namespace Variel.Web.Authentication
{
    public class Credential<TAccount> where TAccount : IAccount
    {
        [Key, Column(Order = 0)]
        public AuthenticationProviders Provider { get; set; }

        [Key, Column(Order = 1)]
        [MaxLength(128)]
        public string ProviderId { get; set; }

        public string Key { get; set; }


        [ForeignKey(nameof(Account))]
        public long AccountId { get; set; }

        [Required]
        public virtual TAccount Account { get; set; }


        public void SetPassword(string password)
        {
            if (Provider == AuthenticationProviders.Self)
                Key = Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string password)
        {
            if (Provider != AuthenticationProviders.Self)
                return true;

            return Crypto.VerifyHashedPassword(Key, password);
        }
    }
}
