namespace VietNOCMS.Models
{
    public class ExploreCourseViewModel
    {
     
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Category> Categories { get; set; } = new List<Category>();

    
        public string SearchQuery { get; set; }
        public int? CategoryId { get; set; }
        public string Level { get; set; } 
        public string SortBy { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

      
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 9;
    }
}
