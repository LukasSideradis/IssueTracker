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
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int IssueId { get; set; }
        [ForeignKey("IssueId")]
        [ValidateNever]
        public Issue Issue { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
    }
}
