using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class ScheduleViewModel
    {
       
        public List<LessonScheduleItem> UpcomingLessons { get; set; } = new List<LessonScheduleItem>();
        public List<LessonScheduleItem> CompletedLessons { get; set; } = new List<LessonScheduleItem>();

      
        public List<AssignmentItem> PendingAssignments { get; set; } = new List<AssignmentItem>();
        public List<AssignmentItem> CompletedAssignments { get; set; } = new List<AssignmentItem>();
    }

    public class LessonScheduleItem
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string LessonName { get; set; }
        public string CourseName { get; set; }
        public string InstructorName { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public string MeetingLink { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime EndTime => StartTime.AddMinutes(DurationMinutes);
        public bool IsLive => DateTime.Now >= StartTime && DateTime.Now <= EndTime;
        public TimeSpan TimeUntilStart => StartTime - DateTime.Now;
    }

    public class AssignmentItem
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public string CourseName { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public string Content { get; set; }
        public string? SubmissionContent { get; set; } 
        public string? SubmissionUrl { get; set; }     

     
        public string TimeRemaining
        {
            get
            {
                if (!DueDate.HasValue) return "Không thời hạn";
                var timeSpan = DueDate.Value - DateTime.Now;
                if (timeSpan.TotalDays < 0) return "Đã quá hạn";
                if (timeSpan.TotalDays >= 1) return $"Còn {(int)timeSpan.TotalDays} ngày";
                if (timeSpan.TotalHours >= 1) return $"Còn {(int)timeSpan.TotalHours} giờ";
                return $"Còn {timeSpan.Minutes} phút";
            }
        }

        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && !IsSubmitted;
    }
}