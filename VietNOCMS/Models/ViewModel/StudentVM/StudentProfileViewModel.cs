using VietNOCMS.Models;

namespace VietNOCMS.ViewModels
{
    public class StudentProfileViewModel
    {
        public User Student { get; set; }
        public List<Enrollment> EnrolledCourses { get; set; } = new List<Enrollment>();

        // Thống kê
        public int TotalCoursesEnrolled { get; set; }
        public int CompletedCourses { get; set; }
        public double AverageProgress { get; set; }
    }
}