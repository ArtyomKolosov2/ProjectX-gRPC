using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectX.Core.Constants;

namespace ProjectX.DataAccess.Models.Identity
{
    public static class IdentityRolesInitializer
    {
        public static async Task EnsureStandardRolesCreated(RoleManager<Role> roleManager)
        {
            var standardRoles = new List<string>
            {
                IdentityRoleConstants.Admin,
                IdentityRoleConstants.User
            };

            foreach (var role in standardRoles)
            {
                await EnsureRoleCreated(roleManager, role);
            }
        }

        private static async Task EnsureRoleCreated(RoleManager<Role> roleManager, string roleName)
        {
            var isRoleExists = await roleManager.RoleExistsAsync(roleName);
            
            if (!isRoleExists)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = roleName
                });
            }
        }
    }
}