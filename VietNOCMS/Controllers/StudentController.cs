using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;
using VietNOCMS.ViewModels;

namespace VietNOCMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int studentId = int.Parse(userIdStr);

          
            var courses = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course).ThenInclude(c => c.Instructor)
                .Include(e => e.Course).ThenInclude(c => c.Category)
                .OrderByDescending(e => e.EnrollmentAt)
                .Select(e => new CourseViewModel
                {
                   
                    CourseId = e.Course.CourseId,
                    CourseName = e.Course.CourseName,
                    Thumbnail = e.Course.Thumbnail,
                    InstructorName = e.Course.Instructor.FullName,
                    CategoryName = e.Course.Category.CategoryName,

                   
                    Progress = e.ProgressPersent,
                    EnrollmentStatus = e.Status // Các trạng thái: 'Pending', 'Approved', 'Rejected'
                })
                .ToListAsync();

            return View(courses);
        }


        public async Task<IActionResult> ExploreCourse(string search, int? categoryId, string level, string sort, int page = 1)
    {
        int pageSize = 9;

     
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

      
        var query = _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Instructor)
            .Include(c => c.Reviews)
            .Include(c => c.Chapters)
            .ThenInclude(ch => ch.Lessons)
            .Where(c => c.IsPublished);

     
        if (!string.IsNullOrEmpty(userIdStr))
        {
            int userId = int.Parse(userIdStr);

       
            var enrolledCourseIds = _context.Enrollments
                .Where(e => e.StudentId == userId)
                .Select(e => e.CourseId);

       
            query = query.Where(c => !enrolledCourseIds.Contains(c.CourseId));
        }
   

      
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c => c.CourseName.Contains(search) ||
                                     c.ShortDescription.Contains(search) ||
                                     c.Instructor.FullName.Contains(search));
        }

      
        if (categoryId.HasValue)
        {
            query = query.Where(c => c.CategoryId == categoryId);
        }

     
        if (!string.IsNullOrEmpty(level))
        {
            query = query.Where(c => c.Level == level);
        }

       
        switch (sort)
        {
            case "price_asc":
                query = query.OrderBy(c => c.Price);
                break;
            case "price_desc":
                query = query.OrderByDescending(c => c.Price);
                break;
            case "rating":
                query = query.OrderByDescending(c => c.EnrollmentCount);
                break;
            default: 
                query = query.OrderByDescending(c => c.CreatedAt);
                break;
        }

     
        int totalItems = await query.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var courses = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var viewModel = new ExploreCourseViewModel
        {
            Courses = courses,
            Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync(),
            SearchQuery = search,
            CategoryId = categoryId,
            Level = level,
            SortBy = sort,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }
    public async Task<IActionResult> Continue()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int studentId = int.Parse(userIdStr);

            var viewModel = new ContinueLearningViewModel();

         
            var enrollments = await _context.Enrollments
                .Where(e => e.StudentId == studentId && e.ProgressPersent < 100)
                .Include(e => e.Course)
                .ThenInclude(c => c.Chapters)
                .ThenInclude(ch => ch.Lessons)
                .Include(e => e.LessonProgresses)
                .OrderByDescending(e => e.EnrollmentAt) 
                .Take(3) 
                .ToListAsync();

            foreach (var enroll in enrollments)
            {
                var lastProgress = enroll.LessonProgresses
                    .OrderByDescending(p => p.LastAccessedAt)
                    .FirstOrDefault();

                
                Lesson currentLesson = null;
                if (lastProgress != null)
                {
                    currentLesson = await _context.Lessons.FindAsync(lastProgress.LessonId);
                }
                else
                {
                   
                    var firstChapter = enroll.Course.Chapters.OrderBy(c => c.OrderIndex).FirstOrDefault();
                    if (firstChapter != null)
                    {
                        currentLesson = firstChapter.Lessons.OrderBy(l => l.OrderIndex).FirstOrDefault();
                    }
                }

                if (currentLesson != null)
                {
                    viewModel.ContinueCourses.Add(new ContinueCourseItem
                    {
                        CourseId = enroll.CourseId,
                        CourseName = enroll.Course.CourseName,
                        LastLessonId = currentLesson.LessonId,
                        LessonName = currentLesson.LessonName,
                        ProgressPercent = enroll.ProgressPersent,
                        RemainingMinutes = (int)(enroll.Course.DurationInHours * 60 * (100 - enroll.ProgressPersent) / 100.0),
                        LastAccessed = lastProgress?.LastAccessedAt ?? enroll.EnrollmentAt
                    });
                }
            }

            var recentProgress = await _context.LessonProgresses
                .Where(p => p.Enrollment.StudentId == studentId)
                .Include(p => p.Lesson)
                .ThenInclude(l => l.Chapter)
                .ThenInclude(ch => ch.Course)
                .OrderByDescending(p => p.LastAccessedAt)
                .Take(5)
                .ToListAsync();

            viewModel.RecentLessons = recentProgress.Select(p => new RecentLessonItem
            {
                LessonId = p.LessonId,
                CourseId = p.Lesson.Chapter.CourseId,
                LessonTitle = p.Lesson.LessonName,
                CourseName = p.Lesson.Chapter.Course.CourseName,
                Duration = p.Lesson.DurationInMinutes + " phút",
               
                ProgressPercent = (p.Lesson.DurationInMinutes > 0)
                    ? (int)((double)p.WatchedDuration / p.Lesson.DurationInMinutes * 100)
                    : 0,
                LastAccessed = p.LastAccessedAt
            }).ToList();

          
            var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1); 
            var completedCount = await _context.LessonProgresses
                .Where(p => p.Enrollment.StudentId == studentId && p.IsCompleted && p.CompleteAt >= startOfWeek)
                .CountAsync();

            viewModel.WeeklyGoal = new WeeklyGoal
            {
                CompletedLessons = completedCount,
                TargetLessons = 10 
            };

        
            viewModel.ScheduleToday = new List<ScheduleItem>
    {
        new ScheduleItem { Day = "Hôm nay", Hour = "20:00", CourseName = "Lập trình C#", LessonTitle = "LINQ nâng cao", Gradient = "linear-gradient(135deg, #667eea, #764ba2)" },
        new ScheduleItem { Day = "Ngày mai", Hour = "19:30", CourseName = "ASP.NET Core", LessonTitle = "Middleware", Gradient = "linear-gradient(135deg, #f093fb, #f5576c)" }
    };

            return View(viewModel);
        }
        [Authorize]
        public async Task<IActionResult> Analytics()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var today = DateTime.Now.Date;

        
            var progressList = await _context.LessonProgresses
                .Include(lp => lp.Lesson).ThenInclude(l => l.Chapter).ThenInclude(c => c.Course).ThenInclude(co => co.Category)
                .Where(lp => lp.Enrollment.StudentId == userId)
                .ToListAsync();

          
            var enrollments = await _context.Enrollments
                .Include(e => e.Course).ThenInclude(c => c.Chapters).ThenInclude(ch => ch.Lessons)
                .Where(e => e.StudentId == userId)
                .ToListAsync();

           
            double totalHours = Math.Round(progressList.Sum(p => p.Lesson.DurationInMinutes) / 60.0, 1);

            int completedLessons = progressList.Count(p => p.IsCompleted);

          
            int totalAssignedLessons = enrollments.Sum(e => e.Course.Chapters.Sum(c => c.Lessons.Count));

            int completionRate = totalAssignedLessons > 0
                ? (int)((double)completedLessons / totalAssignedLessons * 100)
                : 0;

        
            int currentStreak = 0;
            var activeDates = progressList
                .Select(p => p.LastAccessedAt.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

         
            var checkDate = today;
            if (!activeDates.Contains(checkDate)) checkDate = checkDate.AddDays(-1); 

            while (activeDates.Contains(checkDate))
            {
                currentStreak++;
                checkDate = checkDate.AddDays(-1);
            }

         
            var chartLabels = new List<string>();
            var chartValues = new List<double>();

            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                chartLabels.Add(date.DayOfWeek.ToString() == "Sunday" ? "CN" : "T" + ((int)date.DayOfWeek + 1));

            
                var hoursInDay = progressList
                    .Where(p => p.LastAccessedAt.Date == date)
                    .Sum(p => p.Lesson.DurationInMinutes) / 60.0;

                chartValues.Add(hoursInDay);
            }

        
            var categoryGroups = enrollments
                .GroupBy(e => e.Course.Category?.CategoryName ?? "Khác")
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToList();

      
            var viewModel = new StudentAnalyticsViewModel
            {
                TotalStudyHours = totalHours,
                CompletedLessons = completedLessons,
                CompletionRate = completionRate,
                CurrentStreak = currentStreak,

                ChartLabels = chartLabels,
                ChartValues = chartValues,

                CategoryLabels = categoryGroups.Select(x => x.Name).ToList(),
                CategoryData = categoryGroups.Select(x => x.Count).ToList(),

                HasAchievement = currentStreak >= 3 
            };

            return View(viewModel);
        }
     
        public async Task<IActionResult> Schedule()
        {
         
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int studentId = int.Parse(userIdStr);

         
            var enrollments = await _context.Enrollments
                .Include(e => e.Course).ThenInclude(c => c.Instructor)
                .Include(e => e.Course).ThenInclude(c => c.Chapters).ThenInclude(ch => ch.Lessons)
                .ThenInclude(l => l.LessonProgresses.Where(lp => lp.Enrollment.StudentId == studentId))
                .Where(e => e.StudentId == studentId)
                .ToListAsync();

            var viewModel = new ScheduleViewModel();
            var now = DateTime.Now;

         
            foreach (var enrollment in enrollments)
            {
                var lessons = enrollment.Course.Chapters.SelectMany(c => c.Lessons);

                foreach (var lesson in lessons)
                {
                 
                    var progress = lesson.LessonProgresses.FirstOrDefault();
                    bool isCompleted = progress?.IsCompleted ?? false;

                 
                    if (lesson.LessonType == "Meeting" || lesson.LessonType == "Stream")
                    {
                        var scheduleItem = new LessonScheduleItem
                        {
                            LessonId = lesson.LessonId,
                            CourseId = enrollment.CourseId,
                            LessonName = lesson.LessonName,
                            CourseName = enrollment.Course.CourseName,
                            InstructorName = enrollment.Course.Instructor?.FullName ?? "Giảng viên",
                            StartTime = lesson.StartTime ?? DateTime.Now,
                            DurationMinutes = lesson.DurationInMinutes,
                            MeetingLink = lesson.MeetingLink,
                            IsCompleted = isCompleted
                        };

                        if (scheduleItem.EndTime < now)
                            viewModel.CompletedLessons.Add(scheduleItem);
                        else
                            viewModel.UpcomingLessons.Add(scheduleItem);
                    }

              
                    else if (lesson.LessonType == "Assignment" || lesson.DueDate.HasValue)
                    {
                      
                        string contentToShow = null;
                        if (progress != null && !string.IsNullOrEmpty(progress.SubmissionUrl) && progress.SubmissionUrl.EndsWith(".txt"))
                        {
                            try
                            {
                                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, progress.SubmissionUrl.TrimStart('/'));
                                if (System.IO.File.Exists(filePath))
                                {
                                    contentToShow = await System.IO.File.ReadAllTextAsync(filePath);
                                }
                            }
                            catch { }
                        }

                    
                        var assignmentItem = new AssignmentItem
                        {
                            LessonId = lesson.LessonId,
                            LessonName = lesson.LessonName,
                            CourseName = enrollment.Course.CourseName,
                            DueDate = lesson.DueDate,
                            Content = lesson.Content, 
                            IsSubmitted = !string.IsNullOrEmpty(progress?.SubmissionUrl),
                            SubmittedAt = progress?.SubmittedAt,

                           
                            SubmissionContent = contentToShow,
                            SubmissionUrl = progress?.SubmissionUrl
                        };

                      
                        if (assignmentItem.IsSubmitted)
                            viewModel.CompletedAssignments.Add(assignmentItem);
                        else
                            viewModel.PendingAssignments.Add(assignmentItem);
                    }
                }
            }

          
            viewModel.UpcomingLessons = viewModel.UpcomingLessons.OrderBy(x => x.StartTime).ToList();
            viewModel.CompletedLessons = viewModel.CompletedLessons.OrderByDescending(x => x.StartTime).ToList();
            viewModel.PendingAssignments = viewModel.PendingAssignments.OrderBy(x => x.DueDate).ToList();

            return View(viewModel);
        }
        [Authorize] 
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 1. Lấy thông tin user
            var student = await _context.Users.FindAsync(userId);
            if (student == null) return NotFound();

            // 2. Lấy danh sách khóa học đang tham gia (Kèm tiến độ)
            var enrollments = await _context.Enrollments
                .Include(e => e.Course).ThenInclude(c => c.Category)
                .Where(e => e.StudentId == userId)
                .OrderByDescending(e => e.EnrollmentAt)
                .ToListAsync();

            // 3. Tính toán thống kê
            var viewModel = new StudentProfileViewModel
            {
                Student = student,
                EnrolledCourses = enrollments,
                TotalCoursesEnrolled = enrollments.Count,
                CompletedCourses = enrollments.Count(e => e.ProgressPersent == 100),
                AverageProgress = enrollments.Any()
                                  ? Math.Round(enrollments.Average(e => e.ProgressPersent), 1)
                                  : 0
            };

            return View(viewModel);
        }

    }
    }

