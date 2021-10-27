using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices
{
    class UserDBInitialiser
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            // Therapist account
            if (userManager.FindByEmailAsync("mauricederidder@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "mauricederidder@outlook.com",
                };

                IdentityResult result = userManager.CreateAsync(user, "AdminPass21@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user.Id, "Teacher").Wait();
                }
            } 
        }
    }
}
