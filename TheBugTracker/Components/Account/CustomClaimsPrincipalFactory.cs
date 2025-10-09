using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheBugTracker.Client;
using TheBugTracker.Helpers;
using TheBugTracker.Models;

namespace TheBugTracker.Components.Account
{
    public class CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options)
        : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            string profilePictureUrl = user.ProfilePictureId.HasValue 
                ? $"/uploads/{user.ProfilePictureId}" :
                $"https://api.dicebear.com/9.x/glass/svg?seed={user.Id}";

            identity.AddClaims
                ([

                    new Claim(nameof(UserInfo.FirstName), user.FirstName! ),
                    new Claim(nameof(UserInfo.LastName), user.LastName!),
                    new Claim(nameof(UserInfo.CompanyId), user.CompanyId.ToString()),
                    new Claim(nameof(UserInfo.ProfilePictureUrl) , profilePictureUrl),

                ]);
            return identity;
        }
    }
}
