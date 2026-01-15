using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class LessonProgress
    {
        [Key]
        public int ProgressId { get; set; }
      

        public bool IsCompleted { get; set; } = false;
    

        public DateTime? CompleteAt { get; set; }
      

        public int WatchedDuration { get; set; } = 0;
      

        public DateTime LastAccessedAt { get; set; } = DateTime.Now;
    

        public int EnrollmentId { get; set; }
      
        public string? SubmissionUrl { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int? Score { get; set; }           
        public string? InstructorFeedback { get; set; }
        public int LessonId { get; set; }
      
        [ForeignKey("EnrollmentId")]
        public virtual Enrollment Enrollment { get; set; } = null;
    

        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
     
    }
}
