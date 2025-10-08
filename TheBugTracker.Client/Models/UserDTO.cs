using System.ComponentModel.DataAnnotations;
using TheBugTracker.Client.Models.Enums;
using static MudBlazor.CategoryTypes;

namespace TheBugTracker.Client.Models
{
    public class UserDTO
    {
        public required string Id { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public required string? FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public required string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string ImageUrl { get; set; } = $"https://api.dicebear.com/9.x/glass/svg?seed={Random.Shared.Next()}";

        public Role? Role { get; set; }
    }
}
