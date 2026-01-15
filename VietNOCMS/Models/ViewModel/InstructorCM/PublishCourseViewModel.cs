namespace VietNOCMS.Models
{
 
        public class PublishCourseViewModel
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }
            public string ShortDescription { get; set; }
            public string? CourseImageUrl { get; set; }
            public string CategoryName { get; set; }
            public string Level { get; set; }
            public decimal? Price { get; set; }
            public decimal? DiscountPrice { get; set; }
            public string CourseType { get; set; } 


            public int TotalSections { get; set; }
            public int TotalLessons { get; set; }
            public int TotalDuration { get; set; } 
            public int FreePreviewCount { get; set; }

        
            public bool HasBasicInfo => !string.IsNullOrEmpty(CourseName) && !string.IsNullOrEmpty(ShortDescription);
            public bool HasCourseImage => !string.IsNullOrEmpty(CourseImageUrl);
            public bool HasSections => TotalSections >= 1; 
            public bool HasMinimumLessons => TotalLessons >= 1; 
            public bool HasPricing => CourseType == "Free" || (Price.HasValue && Price > 0);

            
            public bool IsReadyToPublish => HasBasicInfo && HasCourseImage && HasSections && HasMinimumLessons && HasPricing;
        }
    }
