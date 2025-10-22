﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TheBugTracker.Client.Models.Enums;

namespace TheBugTracker.Client.Models
{
    public class ProjectDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

      
        [Description("The unique identifier for the project")]
        public int Id { get; set; }


        [Description("The short name or title of a project")]
        [Required]
        public string? Name { get; set; }


        [Description("The detailed user-submitted information abour this project")]
        [Required]
        public string? Description { get; set; }

      
        [Description("The date and time, in UTC, a project was created")]
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }


        [Description("The date and time, in UTC, a project is planned to begin")]
        [Required]
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => _startDate = value?.ToUniversalTime();
        }

        
        [Description("The date and time, in UTC, a project is planned to be completed")]
        [Required]
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => _endDate = value?.ToUniversalTime();
        }


        [Description("The relative priority assigned to a project")]
        public ProjectPriority Priority { get; set; }


        [Description("A flag indicating whether a project is active or archived")]
        public bool Archive { get; set; } = false;


        [Description("A collection of the users assinged to a project")]
        public ICollection<UserDTO> Members { get; set; } = [];


        [Description("A collection of work items or task for this project, such as task or bug reports")]
        public ICollection<TicketDTO> Tickets { get; set; } = [];


        #region helper properties      
        [Required, JsonIgnore]
        public DateTime? StartDateTime 
        { 
            get => StartDate?.DateTime;
            set => StartDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null; 
        }

        [Required, JsonIgnore]
        public DateTime? EndDateTime 
        {
            get => EndDate?.DateTime;
            set => EndDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        #endregion
    }
}
