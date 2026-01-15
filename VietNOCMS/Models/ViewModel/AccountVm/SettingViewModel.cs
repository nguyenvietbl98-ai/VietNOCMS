using System.ComponentModel.DataAnnotations;
namespace VietNOCMS.Models
{
    public class SettingViewModel
    {
        public string? Avatar { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Vai trò")]
        public string Role { get; set; } = string.Empty;

        [Display(Name = "Ngày tạo tài khoản")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Đăng nhập lần cuối")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LastLoginAt { get; set; }
        public List<EnrollmentHistoryItem> PurchaseHistory { get; set; } = new List<EnrollmentHistoryItem>();
        public List<LearningHistoryItem> RecentActivities { get; set; } = new List<LearningHistoryItem>();
     
        public class EnrollmentHistoryItem
        {
            public string CourseName { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string Status { get; set; }
        }

    
        public class LearningHistoryItem
        {
            public string CourseName { get; set; }
            public string LessonName { get; set; }
            public DateTime AccessTime { get; set; }
        }
     
        public string RoleDisplayName
        {
            get
            {
                return Role switch
                {
                    "Admin" => "Quản trị viên",
                    "Instructor" => "Giảng viên",
                    "Student" => "Học viên",
                    _ => "Không xác định"
                };
            }
        }

        public string InitialLetter => !string.IsNullOrEmpty(FullName) ? FullName[0].ToString().ToUpper() : "U";

        public int DaysSinceCreated => (DateTime.Now - CreatedAt).Days;

        public string LastLoginDisplay => LastLoginAt.HasValue
            ? LastLoginAt.Value.ToString("dd/MM/yyyy HH:mm")
            : "Chưa có thông tin";
    }
    public class EditProfileViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0|\+84)[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ (ví dụ: 0912345678)")]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
