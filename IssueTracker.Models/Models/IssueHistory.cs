using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models.Models
{
    public class IssueHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IssueId { get; set; }

        [ForeignKey("IssueId")]
        [ValidateNever]
        public Issue Issue { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
