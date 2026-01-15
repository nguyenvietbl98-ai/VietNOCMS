namespace VietNOCMS.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int PublishedCourses { get; set; }
        public int DraftCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalCategories { get; set; }
        public int PendingInstructorRequests { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int NewCoursesThisMonth { get; set; }

        public List<User> RecentUsers { get; set; } = new();
        public List<Course> RecentCourses { get; set; } = new();
        public List<InstructorRequest> PendingRequests { get; set; } = new();
        public List<Course> PopularCourses { get; set; } = new();
    }
}
