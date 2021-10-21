using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysioOpdrachtIndividueel
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
                    Email = "mauricederidder@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "AdminPass21@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Therapist").Wait();
                }
            }
            // Therapist account
            if (userManager.FindByEmailAsync("timdelaater@outlook.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "timdelaater@outlook.com",
                    Email = "timdelaater@outlook.com",
                    NormalizedEmail = "timdelaater@outlook.com",
                    NormalizedUserName = "timdelaater@outlook.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "AdminPass21@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Therapist").Wait();
                }
            }
        }
    }
}
