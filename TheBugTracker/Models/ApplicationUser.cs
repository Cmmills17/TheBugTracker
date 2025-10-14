using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TheBugTracker.Client.Models;

namespace TheBugTracker.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        // Navigation Properties
        public virtual FileUpload? ProfilePicture { get; set; }

        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }



        public virtual ICollection<Project> Projects { get; set; } = [];


    }

    public static class ApplicationUserExtentions
    {
        public static UserDTO ToDTO(this ApplicationUser user)
        {
            UserDTO dto = new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ProfilePictureId.HasValue
                    ? $"uploads/{user.ProfilePictureId}"
                    : $"https://api.dicebear.com/9.x/glass/svg?seed={user.FirstName}{user.LastName}",


            };
            return dto;
        }
        
    }

}
