using System;
using System.Collections.Generic;

namespace VietNOCMS.Models
{
    public class InstructorAnalyticsViewModel
    {
       
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public double RevenueGrowth { get; set; }
        public int NewStudentsMonth { get; set; } 
        public double AverageRating { get; set; }
        public int TotalRatingCount { get; set; }
        public int TotalStudents { get; set; }

      
        public List<string> ChartLabels { get; set; } = new List<string>();
        public List<decimal> RevenueData { get; set; } = new List<decimal>();

       
        public List<CoursePerformance> TopCourses { get; set; } = new List<CoursePerformance>();

      
        public List<RecentActivity> Activities { get; set; } = new List<RecentActivity>();
    }

    public class CoursePerformance
    {
        public string CourseName { get; set; }
        public int StudentCount { get; set; }
        public decimal Revenue { get; set; }
        public double Percentage { get; set; } 
        public string ColorCode { get; set; }
    }

    public class RecentActivity
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeDisplay { get; set; }
    }
}