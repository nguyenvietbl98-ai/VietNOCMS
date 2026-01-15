using System.ComponentModel.DataAnnotations;

namespace VietNOCMS.ViewModels
{
    public class InstructorEditProfileViewModel
    {
        public string FullName { get; set; }

        public string? Bio { get; set; } 

        public string? CurrentAvatar { get; set; }

        public IFormFile? AvatarFile { get; set; } 
    }
}