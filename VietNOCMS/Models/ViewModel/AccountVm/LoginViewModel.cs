using System.ComponentModel.DataAnnotations;

namespace VietNOCMS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Họ tên chỉ được chứa chữ cái và khoảng trắng")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"^(0|\+84)[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ (ví dụ: 0912345678)")]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Bạn phải chọn vai trò")]
        [Display(Name = "Vai trò")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 kí tự!", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Display(Name = "CV")]
        [DataType(DataType.Upload)]
        public IFormFile? CV { get; set; }

        [Display(Name = "Mô tả kinh nghiệm")]
        public string? InstructorDescription { get; set; }



        [Display(Name = "Tôi đồng ý với Điều khoản sử dụng")]
        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản sử dụng")]

        public bool AgreeToTerms { get; set; }
    }
}
