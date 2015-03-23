using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Claims;
using System.Web;

namespace Identity
{
    public class Anquan
    {
        public static string Getname()
        {
            ClaimsIdentity ci = HttpContext.Current.User.Identity as ClaimsIdentity;
            return ci.Name;
        }
    }
}
