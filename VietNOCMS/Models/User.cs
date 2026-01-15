using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
    
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
      

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
       

        [StringLength(15)]
        public string? PhoneNumber { get; set; }
      
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
     

        public DateTime CreatedAt { get; set; } = DateTime.Now;
     

        public DateTime? LastLoginAt { get; set; }
     

        public bool IsActive { get; set; } = true;
   

        [StringLength(50)]
        public string Role { get; set; } = "Student";
     
        [StringLength(255)]
        public string? Avatar { get; set; }
        public decimal Balance { get; set; } = 0;
        public string? Bio { get; set; }
    }
}
