using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TheBugTracker.Helpers;
using TheBugTracker.Models;

namespace TheBugTracker.Components.Account
{
    public class CustomClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options)
        : UserClaimsPrincipalFactory<ApplicationUser>(userManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            string profilePictureUrl = user.ProfilePictureId.HasValue ? $"/uploads/{user.ProfilePictureId}" : ImageHelper.DefaultProfilePictureUrl;

            identity.AddClaims([
                new Claim("FirstName", user.FirstName! ),
                new Claim("LastName", user.LastName!),
                new Claim("ProfilePictureUrl" , profilePictureUrl)
                ]);

            return identity;
        }
    }
}
