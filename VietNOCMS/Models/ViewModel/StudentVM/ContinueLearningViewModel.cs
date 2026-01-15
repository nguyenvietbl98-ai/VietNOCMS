using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class ContinueLearningViewModel
    {

        public List<ContinueCourseItem> ContinueCourses { get; set; } = new List<ContinueCourseItem>();

  
        public List<RecentLessonItem> RecentLessons { get; set; } = new List<RecentLessonItem>();

      
        public List<ScheduleItem> ScheduleToday { get; set; } = new List<ScheduleItem>();

      
        public WeeklyGoal WeeklyGoal { get; set; } = new WeeklyGoal();
    }

    public class ContinueCourseItem
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int LastLessonId { get; set; }
        public string LessonName { get; set; }
        public int ProgressPercent { get; set; }
        public int RemainingMinutes { get; set; } 
        public DateTime LastAccessed { get; set; }
    }

    public class RecentLessonItem
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string LessonTitle { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public int ProgressPercent { get; set; }
        public DateTime LastAccessed { get; set; }
    }

    public class ScheduleItem
    {
        public string Day { get; set; }
        public string Hour { get; set; }
        public string CourseName { get; set; }
        public string LessonTitle { get; set; }
        public string Gradient { get; set; }
    }

    public class WeeklyGoal
    {
        public int CompletedLessons { get; set; }
        public int TargetLessons { get; set; } = 10; 
    }
}
