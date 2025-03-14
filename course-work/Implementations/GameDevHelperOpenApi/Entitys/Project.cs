using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevHelper.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime CompletionDate { get; set; }

        [Required, MaxLength(50)]
        public string Category { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "In Progress"; // Default status

        // Връзки с другите таблици
        public List<Feature> Features { get; set; } = new List<Feature>();
        public List<BugLog> BugLogs { get; set; } = new List<BugLog>();
    }
}
