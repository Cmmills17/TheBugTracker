using System.ComponentModel.DataAnnotations;

namespace TheBugTracker.Client.Models.Enums
{
    public enum TicketType
    {
        [Display(Name = "New Development")] NewDevelopment,
        [Display(Name = "Work Task")] WorkTask,
        Defect,
        [Display(Name = "Change Request")] ChangeRequest,
        Enhancement,
        [Display(Name = "General Task")] GeneralTask,

    }
}
