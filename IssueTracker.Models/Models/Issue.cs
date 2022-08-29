using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models.Models
{
    public class Issue
    {
        // Tip to self: don't [Required] properties that don't have
        //              input fields, those will be set by the code
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name = "Created by")]
        public string? UserName { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreatedDate { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public string Type { get; set; }
        [Display(Name = "Last updated")]
        public DateTime LastUpdated { get; set; }

    }
}
