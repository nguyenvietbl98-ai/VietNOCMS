using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }
    

        public DateTime EnrollmentAt { get; set; } = DateTime.Now;
    

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
      

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";
     

        [StringLength(100)]
        public string? PaymentMethod { get; set; }
   

        public DateTime? CompleteAt { get; set; }
    
        public int ProgressPersent { get; set; } = 0;
  

        public int StudentId { get; set; }
  

        public int CourseId { get; set; }
      

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; } = null;
      

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null;
    
        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
       
        public string Status { get; set; } = "Pending";


        public string? RejectionReason { get; set; }
        // Điểm tổng kết (GPA)
        public double? FinalScore { get; set; }

        // Xếp loại (Excellent, Good, Pass, Fail...)
        public string? Rank { get; set; }

        // Ngày hoàn thành khóa học (Ngày chốt điểm)
        public DateTime? CompletedAt { get; set; }

        // Trạng thái chứng chỉ (True nếu đã được cấp)
        public bool IsCertified { get; set; } = false;
    }
}
