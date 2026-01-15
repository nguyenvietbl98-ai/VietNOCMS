using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class DashboardViewModel
    {

        public string UserName { get; set; }
        public string Role { get; set; }

        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int PendingInstructorRequests { get; set; }


        public int MyCoursesCount { get; set; }
        public int MyStudentsCount { get; set; }


        public double AverageRating { get; set; }
        public int TotalRatingCount { get; set; }


        public decimal TotalRevenue { get; set; }

        public int EnrolledCoursesCount { get; set; }
        public int InProgressCoursesCount { get; set; }
        public int CompletedCoursesCount { get; set; }
 


        public List<CourseViewModel> DisplayCourses { get; set; } = new List<CourseViewModel>();
        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
   






}
