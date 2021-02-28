using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuthentication.Authentication
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string Role { get; set; }

        public Dictionary<string, dynamic> ToJson()
        {
            return new Dictionary<string, dynamic>()
            {
                {"Id", Id },
                {"UserName", UserName },
                {"FullName", FullName },
                {"Email", Email},
                {"PhoneNumber", PhoneNumber },
                {"Role", Role }
            };
        }
    }
}
