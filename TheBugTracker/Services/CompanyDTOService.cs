using Microsoft.AspNetCore.Identity;
using TheBugTracker.Client;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Models.Enums;
using TheBugTracker.Client.Services.Interfaces;
using TheBugTracker.Models;
using TheBugTracker.Services.Interfaces;

namespace TheBugTracker.Services
{
    public class CompanyDTOService(ICompanyRepository repository,
                                    UserManager<ApplicationUser> userManager) : ICompanyDTOService
    {
        public async Task<IEnumerable<UserDTO>> GetUsersAsync(UserInfo userInfo)
        {
            IEnumerable<ApplicationUser> users = await repository.GetUsersAsync(userInfo);

            List<UserDTO> dtos = [];
            foreach(ApplicationUser user in users)
            {
                UserDTO dto = await user.ToDTOWithRole(userManager);
                dtos.Add(dto);
            }

            return dtos;
        }

        public Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(Role role, UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
