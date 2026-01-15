using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class AddContentViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<SectionViewModel> Sections { get; set; } = new List<SectionViewModel>();
    }

    public class SectionViewModel
    {
        public int ChapterId { get; set; }
        public string SectionTitle { get; set; }
        public int OrderIndex { get; set; }
        public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();

     
        public int CourseId { get; set; }
    }

    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int VideoDuration { get; set; }
        public bool IsFree { get; set; }
        public string? VideoUrl { get; set; }
        public int OrderIndex { get; set; }
        public DateTime? DueDate { get; set; }

    
        public string LessonType { get; set; } 
        public DateTime? StartTime { get; set; }
        public string? MeetingLink { get; set; } 
        public string? Content { get; set; }
    }
}