using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CoursesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

      
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Chapters.OrderBy(ch => ch.OrderIndex))
                    .ThenInclude(ch => ch.Lessons.OrderBy(l => l.OrderIndex))
                .Include(c => c.Reviews.OrderByDescending(r => r.CreateAt))
                    .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(m => m.CourseId == id);

            if (course == null) return NotFound();

          
            ViewBag.AverageRating = course.Reviews.Any()
                ? Math.Round(course.Reviews.Average(r => r.Rating), 1) : 0;
            ViewBag.TotalReviews = course.Reviews.Count;

            bool isOwner = false;
            bool isEnrolled = false;
            bool hasReviewed = false;

            if (User.Identity.IsAuthenticated)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userIdStr))
                {
                    int userId = int.Parse(userIdStr);

                  
                    isOwner = (course.InstructorId == userId);

                  
                    if (!isOwner)
                    {
                        isEnrolled = await _context.Enrollments.AnyAsync(e => e.CourseId == id && e.StudentId == userId);
                        
                        if (isEnrolled)
                        {
                            hasReviewed = await _context.Reviews
                                .AnyAsync(r => r.CourseId == id && r.StudentId == userId);
                        }
                    }
                }
            }

         
            ViewBag.IsOwner = isOwner;
            ViewBag.IsEnrolled = isEnrolled;
            ViewBag.HasReviewed = hasReviewed;

            return View(course);
        }

   
        [Authorize] 
        public async Task<IActionResult> Learn(int id, int? lessonId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = string.IsNullOrEmpty(userIdStr) ? 0 : int.Parse(userIdStr);

         
            var course = await _context.Courses
                .Include(c => c.Chapters.OrderBy(ch => ch.OrderIndex))
                    .ThenInclude(ch => ch.Lessons.OrderBy(l => l.OrderIndex))
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return NotFound();

         
            bool isOwner = (course.InstructorId == userId);
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == id && e.StudentId == userId);

        
            if (!isOwner) 
            {
                if (enrollment == null)
                {
                    return RedirectToAction("Details", new { id = id });
                }

                if (enrollment.Status != "Approved")
                {
                    TempData["ErrorMessage"] = "Yêu cầu tham gia của bạn đang chờ giảng viên phê duyệt.";
                    return RedirectToAction("MyCourses", "Student");
                }
            }

            ViewBag.IsOwner = isOwner;

      
            Lesson currentLesson = null;

         
            if (lessonId.HasValue)
            {
                currentLesson = course.Chapters.SelectMany(c => c.Lessons)
                                               .FirstOrDefault(l => l.LessonId == lessonId);
            }

            if (currentLesson == null && enrollment != null)
            {
                var lastProgress = await _context.LessonProgresses
                    .Where(lp => lp.EnrollmentId == enrollment.EnrollmentId)
                    .OrderByDescending(lp => lp.LastAccessedAt)
                    .FirstOrDefaultAsync();

                if (lastProgress != null)
                {
                    currentLesson = course.Chapters.SelectMany(c => c.Lessons)
                                                   .FirstOrDefault(l => l.LessonId == lastProgress.LessonId);
                }
            }

        
            if (currentLesson == null)
            {
                currentLesson = course.Chapters.OrderBy(c => c.OrderIndex).FirstOrDefault()
                                               ?.Lessons.OrderBy(l => l.OrderIndex).FirstOrDefault();
            }

            if (currentLesson == null) return NotFound("Khóa học này chưa có nội dung.");

        
            var completedLessonIds = new List<int>();
            if (enrollment != null)
            {
                completedLessonIds = await _context.LessonProgresses
                    .Where(lp => lp.EnrollmentId == enrollment.EnrollmentId && lp.IsCompleted)
                    .Select(lp => lp.LessonId)
                    .ToListAsync();

             
                var progress = await _context.LessonProgresses
                    .FirstOrDefaultAsync(lp => lp.EnrollmentId == enrollment.EnrollmentId && lp.LessonId == currentLesson.LessonId);

                if (progress == null)
                {
                    _context.LessonProgresses.Add(new LessonProgress
                    {
                        EnrollmentId = enrollment.EnrollmentId,
                        LessonId = currentLesson.LessonId,
                        LastAccessedAt = DateTime.Now
                    });
                }
                else
                {
                    progress.LastAccessedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

         
            var viewModel = new LearningWatchViewModel
            {
                Course = course,
                CurrentLesson = currentLesson,
                CompletedLessonIds = completedLessonIds,
                EnrollmentId = enrollment?.EnrollmentId ?? 0
            };

            return View("Learn", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitAssignment(int lessonId, string? submissionContent, IFormFile? submissionFile)
        {
            try
            {
                // 1. Lấy User ID
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Vui lòng đăng nhập lại." });
                int userId = int.Parse(userIdStr);

                // 2. Tìm hoặc tạo LessonProgress
                var progress = await _context.LessonProgresses
                    .Include(p => p.Enrollment)
                    .FirstOrDefaultAsync(p => p.LessonId == lessonId && p.Enrollment.StudentId == userId);

                if (progress == null)
                {
                    // Kiểm tra đăng ký khóa học
                    var lesson = await _context.Lessons.Include(l => l.Chapter).FirstOrDefaultAsync(l => l.LessonId == lessonId);
                    var enrollment = await _context.Enrollments
                        .FirstOrDefaultAsync(e => e.CourseId == lesson.Chapter.CourseId && e.StudentId == userId);

                    if (enrollment == null) return Json(new { success = false, message = "Bạn chưa đăng ký khóa học này." });

                    progress = new LessonProgress
                    {
                        EnrollmentId = enrollment.EnrollmentId,
                        LessonId = lessonId,
                        IsCompleted = false,
                        LastAccessedAt = DateTime.Now
                    };
                    _context.LessonProgresses.Add(progress);
                }

                // 3. Xử lý File hoặc Nội dung text
                string finalPath = null;

                if (submissionFile != null && submissionFile.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "submissions");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    // Tạo tên file unique
                    string fileName = $"{userId}_{lessonId}_{Guid.NewGuid()}{Path.GetExtension(submissionFile.FileName)}";
                    string filePath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await submissionFile.CopyToAsync(stream);
                    }
                    finalPath = "/uploads/submissions/" + fileName;
                }
                else if (!string.IsNullOrWhiteSpace(submissionContent))
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "submissions");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string fileName = $"{userId}_{lessonId}_{Guid.NewGuid()}.txt";
                    string filePath = Path.Combine(folder, fileName);

                    await System.IO.File.WriteAllTextAsync(filePath, submissionContent);
                    finalPath = "/uploads/submissions/" + fileName;
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng nhập nội dung hoặc đính kèm file." });
                }

                // 4. Cập nhật Progress
                progress.SubmissionUrl = finalPath;
                progress.SubmittedAt = DateTime.Now;
                progress.IsCompleted = true;

                await _context.SaveChangesAsync(); // Lưu bài nộp

                // ==========================================================
                // --- BẮT ĐẦU: LOGIC GỬI THÔNG BÁO CHO GIẢNG VIÊN ---
                // ==========================================================

                // A. Lấy thông tin cần thiết (Giảng viên ID, Tên bài học, Tên học viên)
                var lessonInfo = await _context.Lessons
                    .Include(l => l.Chapter)
                    .ThenInclude(c => c.Course)
                    .Where(l => l.LessonId == lessonId)
                    .Select(l => new { l.LessonName, l.Chapter.Course.InstructorId })
                    .FirstOrDefaultAsync();

                var studentName = await _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.FullName)
                    .FirstOrDefaultAsync() ?? "Một học viên";

                if (lessonInfo != null)
                {
                    // B. Tạo thông báo
                    await CreateNotification(
                        userId: lessonInfo.InstructorId, // Gửi cho giảng viên
                        title: "Bài nộp mới",
                        message: $"{studentName} vừa nộp bài tập '{lessonInfo.LessonName}'.",
                        type: NotificationType.Warning,  // Màu vàng (Cảnh báo cần chú ý chấm)
                        category: "Assignment",          // Icon Bút chì
                        url: $"/Instructor/GradeLesson?lessonId={lessonId}" // Link đến trang chấm bài (bạn có thể sửa link này cho đúng router của bạn)
                    );

                    await _context.SaveChangesAsync(); // Lưu thông báo
                }
                // ==========================================================
                // --- KẾT THÚC LOGIC THÔNG BÁO ---
                // ==========================================================

                return Json(new { success = true, message = "Nộp bài thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsCompleted(int lessonId, int enrollmentId)
        {
            try
            {

                var progress = await _context.LessonProgresses
                    .FirstOrDefaultAsync(lp => lp.LessonId == lessonId && lp.EnrollmentId == enrollmentId);

                if (progress == null)
                {
                 
                    progress = new LessonProgress
                    {
                        LessonId = lessonId,
                        EnrollmentId = enrollmentId,
                        IsCompleted = false,
                        LastAccessedAt = DateTime.Now
                    };
                    _context.LessonProgresses.Add(progress);
                }

               
                progress.IsCompleted = true;
                progress.CompleteAt = DateTime.Now;
                progress.LastAccessedAt = DateTime.Now;

             
                await _context.SaveChangesAsync();

               
                var enrollment = await _context.Enrollments
                    .Include(e => e.Course)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

                if (enrollment != null)
                {
                   
                    var totalLessons = await _context.Lessons
                        .Include(l => l.Chapter)
                        .Where(l => l.Chapter.CourseId == enrollment.CourseId)
                        .CountAsync();

                   
                    var completedLessons = await _context.LessonProgresses
                        .Where(lp => lp.EnrollmentId == enrollmentId && lp.IsCompleted)
                        .CountAsync();

                   
                    if (totalLessons > 0)
                    {
                        int newPercent = (int)((double)completedLessons / totalLessons * 100);

                       
                        enrollment.ProgressPersent = newPercent;

                    
                        if (newPercent == 100 && enrollment.CompleteAt == null)
                        {
                            enrollment.CompleteAt = DateTime.Now;
                        }

                    
                        await _context.SaveChangesAsync();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

      

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview(int courseId, int rating, string comment)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int studentId = int.Parse(userIdStr);

          
            bool hasEnrolled = await _context.Enrollments
                .AnyAsync(e => e.CourseId == courseId && e.StudentId == studentId);

            if (!hasEnrolled)
            {
                TempData["ErrorMessage"] = "Bạn phải đăng ký khóa học này mới được đánh giá.";
                return RedirectToAction("Details", new { id = courseId });
            }

          
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.CourseId == courseId && r.StudentId == studentId);

            if (existingReview != null)
            {
              
                existingReview.Rating = rating;
                existingReview.Comment = comment;
                existingReview.CreateAt = DateTime.Now;
                TempData["SuccessMessage"] = "Đã cập nhật đánh giá của bạn.";
            }
            else
            {
               
                var review = new Review
                {
                    CourseId = courseId,
                    StudentId = studentId,
                    Rating = rating,
                    Comment = comment,
                    CreateAt = DateTime.Now
                };
                _context.Reviews.Add(review);
                TempData["SuccessMessage"] = "Cảm ơn bạn đã đánh giá!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = courseId });
        }
        private async Task CreateNotification(int userId, string title, string message, NotificationType type, string category, string? url = null)
        {
            var noti = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,       // Enum: Info, Success, Warning, Error
                Category = category, // String: "System", "Payment", "Assignment"...
                RedirectUrl = url,
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _context.Notifications.Add(noti);
            // Lưu ý: Hàm này chưa gọi SaveChangesAsync() để bạn có thể gọi chung với logic chính
        }
    }
}