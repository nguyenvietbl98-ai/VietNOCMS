using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
      

        [Range(1, 5)]
        public int Rating { get; set; }
      

        [StringLength(1000)]
        public string? Comment { get; set; }
     

        public DateTime CreateAt { get; set; } = DateTime.Now;
      

        public int StudentId { get; set; }
       

        public int CourseId { get; set; }
     
        public string? InstructorReply { get; set; }
        public DateTime? ReplyAt { get; set; }     

        [ForeignKey("StudentId")]
        public virtual User Student { get; set; } = null;
      

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null;
      
    }
}
