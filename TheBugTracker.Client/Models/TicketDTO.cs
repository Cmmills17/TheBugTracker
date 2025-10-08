using System.ComponentModel.DataAnnotations;
using TheBugTracker.Client.Models.Enums;

namespace TheBugTracker.Client.Models
{
    public class TicketDTO
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

        public ProjectDTO? Project { get; set; }


        [Required]
        public string? SubmitterUserId { get; set; }

        public UserDTO? SubmitterUser { get; set; }



        public string? DeveloperUserId { get; set; }

        public UserDTO? DeveloperUser { get; set; }

        public ICollection<TicketCommentDTO> Comments { get; set; } = [];

        public ICollection<TicketHistoryDTO> History { get; set; } = [];

        public ICollection<TicketAttachmentDTO> Attatchments { get; set; } = [];
    }
}
