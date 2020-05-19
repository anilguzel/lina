using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LINA.Data.Model;

namespace LINA.API
{
    public class ApplicationInitializer : IApplicationInitializer
    {
        private readonly UserManager<User> _userManager;

        private readonly RoleManager<Role> _roleManager;

        public ApplicationInitializer(
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            var roles = _roleManager.Roles.ToList();
            if (!roles.Any())
            {
                new List<string>
                {
                    "Administrators",
                    "Bot"
                }.ForEach(roleName => _roleManager.CreateAsync(new Role(roleName)).Wait());
            }

            if (!_userManager.Users.Any())
            {
                _userManager.CreateAsync(
                    new User
                    {
                        UserName = "anilguzel",
                        Email = "anilguzel95@gmail.com",
                    }, "LINA123").Wait();

                var user = _userManager.FindByNameAsync("anilguzel").Result;
                _userManager.AddToRolesAsync(user, new[] { "Administrators" }).Wait();
            }
        }
    }

}
