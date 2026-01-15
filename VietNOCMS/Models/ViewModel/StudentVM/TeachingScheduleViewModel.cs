using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class TeachingScheduleViewModel
    {

        public List<TeachingSessionItem> ScheduledSessions { get; set; } = new List<TeachingSessionItem>();

     
        public List<GradingItem> PendingGrading { get; set; } = new List<GradingItem>();

      
        public List<CourseCompactViewModel> MyCourses { get; set; } = new List<CourseCompactViewModel>();
    }

    public class TeachingSessionItem
    {
        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string CourseName { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public int StudentCount { get; set; } 
        public string MeetingLink { get; set; }
        public string DocumentUrl { get; set; }

        public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);

   
        public bool IsLive => DateTime.Now >= StartTime && DateTime.Now <= EndTime;
        public bool IsUpcoming => DateTime.Now < StartTime;
        public string TimeStatus => IsLive ? "Đang diễn ra" : (IsUpcoming ? "Sắp diễn ra" : "Đã kết thúc");
    }

    public class GradingItem
    {
        public int LessonId { get; set; }
        public string AssignmentTitle { get; set; } 
        public string CourseName { get; set; }
        public DateTime? DueDate { get; set; }

        public int TotalStudents { get; set; }
        public int SubmittedCount { get; set; }
        public int GradedCount { get; set; }
        public string AssignmentContent { get; set; }
 
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now;
        public int PendingCount => SubmittedCount - GradedCount; 
    }
}