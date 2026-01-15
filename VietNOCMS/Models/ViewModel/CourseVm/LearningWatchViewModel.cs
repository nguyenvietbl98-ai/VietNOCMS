namespace VietNOCMS.Models
{
    public class LearningWatchViewModel
    {
      
      
            public Course Course { get; set; } 
            public Lesson CurrentLesson { get; set; } 
            public int EnrollmentId { get; set; }

        
            public List<int> CompletedLessonIds { get; set; } = new List<int>();

        
            public int TotalLessons => Course?.Chapters?.Sum(c => c.Lessons.Count) ?? 0;

      
            public int CompletedCount => CompletedLessonIds?.Count ?? 0;

          
            public int ProgressPercent
            {
                get
                {
                    if (TotalLessons == 0) return 0;
                    return (int)((double)CompletedCount / TotalLessons * 100);
                }
            }

         
            public bool IsCurrentLessonCompleted => CompletedLessonIds.Contains(CurrentLesson?.LessonId ?? 0);
        }
    
}

