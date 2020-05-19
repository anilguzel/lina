using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace LINA.Data.Model
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
        }

        public Role(string roleName)
            : base(roleName)
        {
        }
    }
}
