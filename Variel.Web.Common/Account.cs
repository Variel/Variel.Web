using System;

namespace Variel.Web.Common
{
    public interface IAccount
    {
        long Id { get; set; }
        string Name { get; set; }
    }
}
