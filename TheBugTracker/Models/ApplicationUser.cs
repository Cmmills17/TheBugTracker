using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

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

}
