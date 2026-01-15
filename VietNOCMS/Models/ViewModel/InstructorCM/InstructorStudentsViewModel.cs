using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
   
    public class InstructorStudentsViewModel
    {
    
        public List<CourseCompactViewModel> Courses { get; set; } = new List<CourseCompactViewModel>();

     
        public int? SelectedCourseId { get; set; }

     
        public List<StudentEnrolledViewModel> Students { get; set; } = new List<StudentEnrolledViewModel>();
    }


    public class CourseCompactViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
    }

 
    public class StudentEnrolledViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string? StudentAvatar { get; set; } 
        public string? PhoneNumber { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }
        public int ProgressPercent { get; set; }
        public DateTime? LastActive { get; set; } 

        public string Status
        {
            get
            {
                if (ProgressPercent == 100) return "Completed";
                if (ProgressPercent > 0) return "InProgress";   
                return "New";                                   
            }
        }
    }
}