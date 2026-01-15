using System.ComponentModel.DataAnnotations;

namespace VietNOCMS.Models {
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả ngắn")]
        public string ShortDescription { get; set; }
        public int? CourseId { get; set; } 
        public string? CurrentThumbnail { get; set; } 

     
        public string? DetailedDescription { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn cấp độ")]
        public string Level { get; set; } = "Beginner";

   
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? CourseImage { get; set; }

    
        public string CourseType { get; set; } = "Free";

        public decimal? Price { get; set; }

        public decimal? DiscountPrice { get; set; } 

        public double EstimatedDuration { get; set; }

        public string Language { get; set; } = "Tiếng Việt";

        public string? Requirements { get; set; }

        public string? WhatYouWillLearn { get; set; }
        [Display(Name = "Tên chủ đề mới")]
        public string? NewCategoryName { get; set; }
 
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}