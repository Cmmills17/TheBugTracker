using System.ComponentModel.DataAnnotations;

namespace TheBugTracker.Client.Models.Enums
{
    public enum Role
    {
        Admin,
        [Display(Name = "Project Manager")] ProjectManager,
        Developer,
        Submitter,
        [Display(Name = "Demo User")] DemoUser,
    }
}
