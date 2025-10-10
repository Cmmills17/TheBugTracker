using System.ComponentModel.DataAnnotations;
using TheBugTracker.Client.Models.Enums;

namespace TheBugTracker.Client.Models
{
    public class ProjectDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }


        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        [Required]
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => _startDate = value?.ToUniversalTime();
        }

        [Required]
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => _endDate = value?.ToUniversalTime();
        }

        public ProjectPriority Priority { get; set; }

        public bool Archive { get; set; } = false;

        public ICollection<UserDTO> Members { get; set; } = [];

        public ICollection<TicketDTO> Tickets { get; set; } = [];

        #region helper properties

        [Required]
        public DateTime? StartDateTime 
        { 
            get => StartDate?.DateTime;
            set => StartDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null; 
        }

        [Required]
        public DateTime? EndDateTime 
        {
            get => EndDate?.DateTime;
            set => EndDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        #endregion
    }
}
