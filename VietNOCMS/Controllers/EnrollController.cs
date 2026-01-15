using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class EnrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollController(ApplicationDbContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public async Task<IActionResult> Checkout(int courseId)
        {
          
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdStr);

        
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.CourseId == courseId && c.IsPublished);

            if (course == null)
            {
                return NotFound("Khóa học không tồn tại hoặc chưa được xuất bản.");
            }

          
            bool isAlreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.CourseId == courseId && e.StudentId == userId);

            if (isAlreadyEnrolled)
            {
                TempData["SuccessMessage"] = "Bạn đã đăng ký khóa học này rồi. Hãy tiếp tục học!";
                return RedirectToAction("MyCourses", "Student");
            }

            var viewModel = new CourseViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Thumbnail = course.Thumbnail,
                CategoryName = course.Category?.CategoryName ?? "Chung",
                Level = course.Level,
                InstructorName = course.Instructor?.FullName ?? "Admin",

                OriginalPrice = course.Price,
                DiscountPercent = course.DiscountPercent,
                IsPaid = course.Price > 0,

                DurationInHours = course.DurationInHours
            };

            return View(viewModel);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int courseId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int studentId = int.Parse(userIdStr);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var course = await _context.Courses.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.CourseId == courseId);
                var student = await _context.Users.FindAsync(studentId);

               
                decimal finalPrice = course.DiscountPercent > 0
                    ? course.Price * (100 - course.DiscountPercent.Value) / 100
                    : course.Price;

               
                if (student.Balance < finalPrice)
                {
                    TempData["ErrorMessage"] = "Số dư không đủ!";
                    return RedirectToAction("Index", "Wallet");
                }

            
                student.Balance -= finalPrice;
                _context.Wallet.Add(new Wallet
                {
                    UserId = studentId,
                    Amount = -finalPrice,
                    Type = "Purchase",
                    Description = $"Đăng ký khóa học (Chờ duyệt): {course.CourseName}",
                    Status = "Completed",
                    CreatedAt = DateTime.Now
                });

             
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    StudentId = studentId,
                    PaidAmount = finalPrice,
                    PaymentStatus = "Paid",
                    Status = "Pending", 
                    EnrollmentAt = DateTime.Now,
                    ProgressPersent = 0
                };
                _context.Enrollments.Add(enrollment);

                // Gửi thông báo cho giảng viên về đăng ký mới
                _context.Notifications.Add(new Notification
                {
                    UserId = course.InstructorId,
                    Title = "Đăng ký khóa học mới",
                    Message = $"{student.FullName} vừa đăng ký khóa học '{course.CourseName}'. Vui lòng duyệt yêu cầu.",
                    Type = NotificationType.Info,
                    Category = "Course",
                    RedirectUrl = "/Instructor/ManageEnrollments",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

               
                HttpContext.Session.SetString("UserBalance", student.Balance.ToString("N0"));

                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng chờ giảng viên phê duyệt.";
                return RedirectToAction("MyCourses", "Student");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return RedirectToAction("Error", "Home");
            }
        }
     
      
    }
}