using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Variel.Web.Extensions
{
    public static class SessionExtension
    {
        public static void SetInt64(this ISession _this, string key, long value)
        {
            _this.Set(key, BitConverter.GetBytes(value));
        }

        public static long? GetInt64(this ISession _this, string key)
        {
            var val = _this.Get(key);
            if (val == null || val.Length == 0)
                return null;

            return BitConverter.ToInt64(val, 0);
        }
    }
}
