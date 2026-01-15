using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class InstructorRequest
    {
        [Key]
        public int RequestId { get; set; }
    

        [Required]
        [StringLength(500)]
        public string Bio { get; set; } = string.Empty;
       

        [StringLength(500)]
        public string? CvUrl { get; set; }
       

        [StringLength(200)]
        public string? LinkedInProfile { get; set; }
      

        [StringLength(200)]
        public string? FacebookProfile { get; set; }
     
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
  
        public string? RejectionReason { get; set; }
    
        public DateTime RequestedAt { get; set; } = DateTime.Now;
    

        public DateTime? ReviewedAt { get; set; }
      

        public int? ReviewedByAdminId { get; set; }
      
        public int UserId { get; set; }
     

    
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
     

        [ForeignKey("ReviewedByAdminId")]
        public virtual User? ReviewedByAdmin { get; set; }
     
    }
}
