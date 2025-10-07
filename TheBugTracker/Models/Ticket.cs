using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using TheBugTracker.Client.Models.Enums;

namespace TheBugTracker.Models
{
    public class Ticket
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;

        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTimeOffset Created 
        { 
            get => _created; 
            set => _created = value.ToUniversalTime(); 
        }

        public DateTimeOffset? Updated 
        { 
    
            get => _updated;
            set
            {
                if (value.HasValue)
                {
                    _updated = value.Value;
                }
                else
                {
                    _updated = null;
                }
                //  _updated = ValueTask.HasValue ? value.Value.ToUniversalTime() : null;
                // Can also use this instead of an if statement
            }
        }

        public bool Archived { get; set; }

        public bool ArchivedByProject { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketType Type { get; set; }

        public TicketStatus Status { get; set; }


        // navigation properties
        public int ProjectId { get; set; }
        
        public virtual Project? Project { get; set; }


        [Required]
        public string? SubmitterUserId { get; set; }

        public virtual ApplicationUser? SubmitterUser { get; set; }



        public string? DeveloperUserId { get; set; }

        public virtual ApplicationUser? DeveloperUser { get; set; }


        
        public virtual ICollection<TicketComment> Comments { get; set; } = [];

        public virtual ICollection<TicketHistory> History { get; set; } = [];

        public virtual ICollection<TicketAttatchment> Attatchments { get; set; } = [];


    }
}
