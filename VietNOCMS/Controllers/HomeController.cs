using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;


namespace VietNOCMS.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

     
        public IActionResult Index()
        {
        
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

   
        [Authorize] 
        public async Task<IActionResult> Dashboard()
        {
          
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account"); 
            var user = await _context.Users.FindAsync(int.Parse(userIdStr));
            var userId = int.Parse(userIdStr);
            var userRole = User.IsInRole("Admin") ? "Admin" : (User.IsInRole("Instructor") ? "Instructor" : "Student");
            var userName = User.Identity.Name;
            HttpContext.Session.SetString("UserBalance", user.Balance.ToString("N0"));

     
            var viewModel = new DashboardViewModel
            {
                UserName = userName,
                Role = userRole
            };

        

            if (userRole == "Admin")
            {
                
                viewModel.TotalUsers = await _context.Users.CountAsync();
                viewModel.TotalCourses = await _context.Courses.CountAsync();
                viewModel.TotalEnrollments = await _context.Enrollments.CountAsync();
                viewModel.PendingInstructorRequests = await _context.InstructorRequests.CountAsync(r => r.Status == "Pending");

           
                viewModel.DisplayCourses = await _context.Courses
                    .Include(c => c.Category) 
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .Select(c => new CourseViewModel
                    {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        Thumbnail = c.Thumbnail,
                        CategoryName = c.Category.CategoryName,
                        Level = c.Level,
                        StudentCount = c.EnrollmentCount,
                        Rating = 4.5 
                    }).ToListAsync();
            }
            else if (userRole == "Instructor")
            {
             
                var myCoursesQuery = _context.Courses.Where(c => c.InstructorId == userId);

                viewModel.MyCoursesCount = await myCoursesQuery.CountAsync();
                viewModel.MyStudentsCount = await myCoursesQuery.SumAsync(c => c.EnrollmentCount);

              
                var courseIds = await myCoursesQuery.Select(c => c.CourseId).ToListAsync();
                if (courseIds.Any())
                {
                    var paidAmounts = await _context.Enrollments
                        .Where(e => courseIds.Contains(e.CourseId) && e.PaymentStatus == "Completed")
                        .Select(e => e.PaidAmount)
                        .ToListAsync();

                    viewModel.TotalRevenue = paidAmounts?.Sum() ?? 0;
                }
                else
                {
                    viewModel.TotalRevenue = 0;
                }
                viewModel.Notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)           
                    .OrderByDescending(n => n.CreatedAt)       
                    .Take(3)                                  
                    .ToListAsync();

                var reviewQuery = _context.Reviews.Where(r => courseIds.Contains(r.CourseId));
                viewModel.TotalRatingCount = await reviewQuery.CountAsync();
                viewModel.AverageRating = viewModel.TotalRatingCount > 0
                    ? Math.Round(await reviewQuery.AverageAsync(r => (double)r.Rating), 1)
                    : 0;

             
                var latestCourses = await myCoursesQuery
                    .Include(c => c.Category)
                    .Include(c => c.Reviews)
                    .OrderByDescending(c => c.CreatedAt)
                    .Take(3)
                    .ToListAsync();

                viewModel.DisplayCourses = latestCourses.Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Thumbnail = c.Thumbnail,
                    CategoryName = c.Category?.CategoryName ?? "Chưa phân loại",
                    Level = c.Level,
                    StudentCount = c.EnrollmentCount,
                    Rating = c.Reviews != null && c.Reviews.Any() ? Math.Round(c.Reviews.Average(r => r.Rating), 1) : 0
                }).ToList();
            }
            else // Student
            {
                // Load notifications for Student
                viewModel.Notifications = await _context.Notifications
                    .Where(n => n.UserId == userId)            // Chỉ lấy thông báo của user này
                    .OrderByDescending(n => n.CreatedAt)       // Thông báo mới nhất lên đầu
                    .Take(10)                                  // Chỉ lấy 10 thông báo gần nhất để tránh lag
                    .ToListAsync();
                var enrollmentsQuery = _context.Enrollments.Where(e => e.StudentId == userId);

                viewModel.EnrolledCoursesCount = await enrollmentsQuery.CountAsync();
                viewModel.CompletedCoursesCount = await enrollmentsQuery.CountAsync(e => e.CompleteAt != null);
                viewModel.InProgressCoursesCount = viewModel.EnrolledCoursesCount - viewModel.CompletedCoursesCount;

              
                viewModel.DisplayCourses = await enrollmentsQuery
                    .Include(e => e.Course)
                    .ThenInclude(c => c.Category) 
                    .OrderByDescending(e => e.EnrollmentAt)
                    .Take(3)
                    .Select(e => new CourseViewModel
                    {
                        CourseId = e.CourseId,
                        CourseName = e.Course.CourseName,
                        Thumbnail = e.Course.Thumbnail,
                        CategoryName = e.Course.Category.CategoryName,
                        Level = e.Course.Level,
                        Progress = e.ProgressPersent
                    }).ToListAsync();
            }

            return View(viewModel); 
        }

      
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}