namespace VietNOCMS.ViewModels
{
    public class InstructorProfileViewModel
    {
        public Models.User Instructor { get; set; } = null!;
        public List<Models.Course> Courses { get; set; } = new();
        public int TotalStudents { get; set; }
    }
}