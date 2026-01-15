using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
     
        [Required]
        [StringLength(200)]
        public string LessonName { get; set; } = string.Empty;
      

        [StringLength(50)]
        public string LessonType { get; set; } = "Video";
    

        [StringLength(500)]
        public string? VideoUrl { get; set; }
      
        public string? Content { get; set; }
      

        [StringLength(500)]
        public string? DocumentUrl { get; set; }
     

        public int DurationInMinutes { get; set; } = 0;
      

        public int OrderIndex { get; set; }
      

        public bool IsFree { get; set; } = false;
     

        public DateTime CreateAt { get; set; } = DateTime.Now;
     

        public DateTime? StartTime { get; set; }
      

        [StringLength(500)]
        public string? MeetingLink { get; set; }
   
        public DateTime? DueDate { get; set; }
        public int? MaxScore { get; set; }

        public int ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        [NotMapped] 
        public bool IsAssignment => LessonType == "Assignment";

        [NotMapped]
        public bool IsMeeting => LessonType == "Meeting" || LessonType == "Stream";
        public virtual Chapter Chapter { get; set; } = null;
     

        public virtual ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
      
    }
}
