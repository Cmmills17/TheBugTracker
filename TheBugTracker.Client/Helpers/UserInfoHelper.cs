using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using TheBugTracker.Client.Models;

namespace TheBugTracker.Client.Helpers
{
    public static class UserInfoHelper
    {
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
                return new UserInfo
                {
                    UserId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                    Email = user.FindFirst(ClaimTypes.Email)!.Value,
                    Firstname = user.FindFirst("FirstName")!.Value,
                    Lastname = user.FindFirst("LastName")!.Value,
                    ProfilePictureUrl = user.FindFirst(nameof(UserInfo.ProfilePictureUrl))!.Value
                };

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
