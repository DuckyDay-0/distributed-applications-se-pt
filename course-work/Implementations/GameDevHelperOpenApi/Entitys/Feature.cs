using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameDevHelper.Models
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required, MaxLength(50)]
        public string Status { get; set; } = "In Progress"; // Default status

        //Connection with the project
        public int ProjectId { get; set; }

        [ValidateNever]
        public Project Project { get; set; }

        // User assigned to work on the feature
        public string? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
