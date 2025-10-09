using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace TheBugTracker.Client.Helpers
{
    public static class UserInfoHelper
    {
        // Claims come from:
        // - Task<AuthenticationState>
        // - AuthenticationState
        // - ClaimsPrincipal

        public static async Task<UserInfo?> GetUserInfoAsync(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask is null) return null;

            AuthenticationState authState = await authStateTask;
            return GetUserInfo(authState.User);

        }

        public static UserInfo? GetUserInfo(AuthenticationState authState)
        {
            ClaimsPrincipal user = authState.User;
            return GetUserInfo(user);

        }

        public static UserInfo? GetUserInfo(ClaimsPrincipal user)
        {
            try
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                var email = user.FindFirst(ClaimTypes.Email)!.Value;
                var firstName = user.FindFirst(nameof(UserInfo.FirstName))!.Value;
                var lastName = user.FindFirst(nameof(UserInfo.LastName))!.Value;
                var companyId = user.FindFirst(nameof(UserInfo.CompanyId))!.Value;
                var profilePictureUrl = user.FindFirst(nameof(UserInfo.ProfilePictureUrl))!.Value;
                var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);


                return new UserInfo
                {
                    UserId = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    CompanyId = int.Parse(companyId),
                    Email = email,
                    ProfilePictureUrl = profilePictureUrl,
                    Roles = [.. roles],

                };
            }
            catch
            {
                return null;
            }

        }


    }
}
