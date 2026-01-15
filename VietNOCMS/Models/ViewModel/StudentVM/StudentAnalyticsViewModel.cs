using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class StudentAnalyticsViewModel
    {
    
        public double TotalStudyHours { get; set; } 
        public int CompletedLessons { get; set; } 
        public int CompletionRate { get; set; }    
        public int CurrentStreak { get; set; }    


        public List<string> ChartLabels { get; set; } = new List<string>(); 
        public List<double> ChartValues { get; set; } = new List<double>(); 

    
        public List<string> CategoryLabels { get; set; } = new List<string>();
        public List<int> CategoryData { get; set; } = new List<int>();

  
        public string StudyTip { get; set; } = "Hãy cố gắng học đều đặn mỗi ngày!";
        public bool HasAchievement { get; set; }
    }
}