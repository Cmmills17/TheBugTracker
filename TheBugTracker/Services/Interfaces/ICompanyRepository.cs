using TheBugTracker.Client;
using TheBugTracker.Client.Models.Enums;
using TheBugTracker.Models;

namespace TheBugTracker.Services.Interfaces
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Get all users in the current users company
        /// </summary>
        /// <param name="userInfo">The current user's claims</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersAsync(UserInfo userInfo);

        /// <summary>
        /// Gett all users un the current user's company
        /// in a specific role
        /// </summary>
        /// <param name="role">The role assigned to the users</param>
        /// <param name="userInfo">The current user's claims</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(Role role, UserInfo userInfo);
    }
}
