using System.ComponentModel.DataAnnotations;

namespace VietNOCMS.Models
{
   
    public class InstructorRequestListViewModel
    {
        public int RequestId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? CvUrl { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? FacebookProfile { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
    public class InstructorRequestViewModel
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; } 
        public string CvUrl { get; set; }
        public string Status { get; set; } 
        public DateTime RequestedAt { get; set; }

      
        public string? LinkedIn { get; set; }
        public string? Facebook { get; set; }
    }

    public class ReviewInstructorRequestViewModel
    {
        public int RequestId { get; set; }

        [Required]
        public string Action { get; set; } = string.Empty; // "Approve" or "Reject"

        [Display(Name = "Lý do từ chối")]
        [StringLength(500)]
        public string? RejectionReason { get; set; }
    }
}
