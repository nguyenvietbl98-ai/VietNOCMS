using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Chapter
    {
        [Key]
        public int ChapterId { get; set; }
   

        [Required]
        [StringLength(200)]
        public string ChapterName { get; set; } = string.Empty;
   

        [StringLength(500)]
        public string? Description { get; set; }
       

        public int OrderIndex { get; set; }
     
        public DateTime CreateAt { get; set; } = DateTime.Now;
     
        public int CourseId { get; set; }
  

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null;
  

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
     
    }
}
