using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameDevHelper.Models
{
    public class BugLog
    {
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "Open"; // Default status

        // Връзка с Project (Много към 1)
        public int ProjectId { get; set; }

        [ValidateNever]
        public Project Project { get; set; }

        // Кой работи върху бъга (1 към много)
        public string? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
