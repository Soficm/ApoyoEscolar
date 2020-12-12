using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apoyo.Client.Auth
{
    interface ILoginServices
    {
        Task Login(string token);
        Task Logout();
    }
}
