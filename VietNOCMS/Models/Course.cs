using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace VietNOCMS.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
     
        [Required]
        [StringLength(200)]
        public string CourseName { get; set; } = string.Empty;
  

        [Required]
        public string ShortDescription { get; set; } = string.Empty;
       
     

        [StringLength(500)]
        public string? Thumbnail { get; set; }
   
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { set; get; }
  
        public string? Requirements { get; set; }
     
        public string? WhatYouWillLearn { get; set; }
   
        public int? DiscountPercent { get; set; }
    

        [StringLength(50)]
        public string Level { get; set; } = "Beginner";
    

        [StringLength(50)]
        public string Language { get; set; } = "Tiếng Việt";
     

        public int DurationInHours { get; set; }
   

        public bool IsPublished { get; set; } = false;
      

        public int ViewCount { get; set; } = 0;
    

        public int EnrollmentCount { get; set; } = 0;
     

        public DateTime CreatedAt { get; set; } = DateTime.Now;
     

        public DateTime? UpdateAt { get; set; }
     
        public int InstructorId { get; set; }
  
        public string? Description { get; set; }

        public int CategoryId { get; set; }
     

        [ForeignKey("InstructorId")]
        public virtual User Instructor { get; set; } = null;
   

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null;
     

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
     

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
       

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CourseCollaborator> Collaborators { get; set; } = new List<CourseCollaborator>();
        public bool IsRecruiting { get; set; } = false; 


    }
}
