using System; 

namespace VietNOCMS.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? Thumbnail { get; set; }
        public string CategoryName { get; set; } = "General";
        public string Level { get; set; } = "Beginner";
        public string InstructorName { get; set; } = string.Empty;

    
        public decimal OriginalPrice { get; set; }
        public int? DiscountPercent { get; set; }
        public bool IsPaid { get; set; }

      
        public decimal FinalPrice
        {
            get
            {
                if (!IsPaid) return 0;
                if (DiscountPercent.HasValue && DiscountPercent.Value > 0)
                {
                    return OriginalPrice * (100 - DiscountPercent.Value) / 100;
                }
                return OriginalPrice;
            }
        }

     
        public double Rating { get; set; }
        public int StudentCount { get; set; }
        public int Progress { get; set; } 
        public int DurationInHours { get; set; }
        public int LessonCount { get; set; }

   
        public bool IsPublished { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsOwner { get; set; }
        public string EnrollmentStatus { get; set; }
      
    }
}