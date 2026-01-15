using System.ComponentModel.DataAnnotations;

namespace VietNOCMS.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
    

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;


        [StringLength(2000)]
        public string Description { get; set; }
     

        [StringLength(50)]
        public string? Icon { get; set; }
  

        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; } = DateTime.Now;
 
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
      
    }
}
