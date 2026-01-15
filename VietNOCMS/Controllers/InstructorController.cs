using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;
using VietNOCMS.Services;
using VietNOCMS.ViewModels;
using static System.Collections.Specialized.BitVector32;

namespace VietNOCMS.Controllers
{
    [Authorize(Roles = "Instructor, Student")] 
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context; 
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly VietNOCMS.Services.GeminiService _geminiService;
        private readonly ICourseAuthorizationService _authService;
        public InstructorController(ApplicationDbContext context, ICourseAuthorizationService authService, IWebHostEnvironment webHostEnvironment, VietNOCMS.Services.GeminiService geminiService)
        {
            _context = context;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
            _geminiService = geminiService;
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet]
        public async Task<IActionResult> CreateCourse()
        {
            var viewModel = new CreateCourseViewModel
            {
              
                Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCourse(CreateCourseViewModel model, string action)
        {
            // 1. XỬ LÝ LOGIC TẠO DANH MỤC MỚI (THÊM ĐOẠN NÀY)
            if (model.CategoryId == -1)
            {
                if (string.IsNullOrWhiteSpace(model.NewCategoryName))
                {
                    ModelState.AddModelError("NewCategoryName", "Vui lòng nhập tên chủ đề mới.");
                }
                else
                {
                    // Kiểm tra xem tên này đã tồn tại chưa để tránh trùng lặp
                    var existingCat = await _context.Categories
                        .FirstOrDefaultAsync(c => c.CategoryName.ToLower() == model.NewCategoryName.Trim().ToLower());

                    if (existingCat != null)
                    {
                        // Nếu có rồi thì dùng luôn ID của nó
                        model.CategoryId = existingCat.CategoryId;
                    }
                    else
                    {
                        // Nếu chưa có thì Tạo mới vào DB
                        var newCategory = new Category
                        {
                            CategoryName = model.NewCategoryName.Trim(),
                            Description = "Được tạo bởi giảng viên",
                            IsActive = true,
                            CreateAt = DateTime.Now,
                            Icon = "bi-folder" // Icon mặc định
                        };

                        _context.Categories.Add(newCategory);
                        await _context.SaveChangesAsync(); // Lưu để lấy ID

                        // Gán ID mới tạo vào model để dùng cho khóa học bên dưới
                        model.CategoryId = newCategory.CategoryId;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                // 1. Xử lý Upload Ảnh
                string uniqueFileName = null;
                if (model.CourseImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    // Tạo thư mục nếu chưa có
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CourseImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CourseImage.CopyToAsync(fileStream);
                    }
                }

                // 2. Tính toán Logic Giá & Giảm giá
                decimal finalPrice = 0;
                int? discountPercent = null;

                if (model.CourseType == "Paid" && model.Price.HasValue)
                {
                    finalPrice = model.Price.Value;

                    // Tính % giảm giá nếu có nhập giá khuyến mãi
                    if (model.DiscountPrice.HasValue && model.DiscountPrice.Value < finalPrice)
                    {
                        // Công thức: % Giảm = (1 - (Giá giảm / Giá gốc)) * 100
                        discountPercent = (int)((1 - (model.DiscountPrice.Value / finalPrice)) * 100);
                    }
                }

                // 3. Lấy ID giảng viên hiện tại (Giả lập logic lấy User ID)
                // int currentInstructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int currentInstructorId = int.Parse(userIdClaim); // Tạm thời hardcode để test

                // 4. Map ViewModel sang Model Entity
                var course = new Course
                {
                    CourseName = model.CourseName,
                    ShortDescription = model.ShortDescription,
                    // Description = model.DetailedDescription, // Bỏ comment dòng này nếu bạn đã thêm cột Description vào DB
                    CategoryId = model.CategoryId,
                    Level = model.Level,
                    Language = model.Language,
                    Thumbnail = uniqueFileName, // Lưu tên file ảnh

                    Price = finalPrice,
                    DiscountPercent = discountPercent,

                    DurationInHours = (int)model.EstimatedDuration, // Model gốc là int, ViewModel là double
                    Requirements = model.Requirements,
                    WhatYouWillLearn = model.WhatYouWillLearn,

                    InstructorId = currentInstructorId,
                    CreatedAt = DateTime.Now,
                    IsPublished = false, // Mặc định là nháp
                    ViewCount = 0,
                    EnrollmentCount = 0
                };

                // 5. Lưu vào Database
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                // 6. Điều hướng dựa trên nút bấm (Lưu nháp hay Tiếp theo)
                if (action == "next")
                {
                    // Chuyển sang trang thêm nội dung (Chapter/Lesson)
                    return RedirectToAction("AddContent", new { id = course.CourseId });
                }
                else
                {
                    // Quay về danh sách khóa học
                    return RedirectToAction("MyCourses");
                }
            }

            // Nếu lỗi validation, load lại danh sách Category để không bị null view
            model.Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> MyCourses(string search)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 1. Query lấy danh sách khóa học (Cả sở hữu và được mời)
            var query = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Collaborators)
                // [QUAN TRỌNG] Phải include Chapters và Reviews để tính số bài học và điểm đánh giá
                .Include(c => c.Chapters).ThenInclude(ch => ch.Lessons)
                .Include(c => c.Reviews)
                .Where(c =>
                    c.InstructorId == userId || // Trường hợp 1: Là chủ khóa học
                    c.Collaborators.Any(cb => cb.CollaboratorId == userId && cb.Status == "Accepted") // Trường hợp 2: Là trợ giảng
                );

            // 2. Lọc theo từ khóa tìm kiếm (nếu có)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.CourseName.Contains(search));
            }

            var courses = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();

            // 3. Tính toán thống kê Dashboard
            // Lưu ý: Chỉ tính doanh thu cho các khóa mình làm Chủ sở hữu (InstructorId == userId)
            var myOwnedCourses = courses.Where(c => c.InstructorId == userId).ToList();

            var viewModel = new DashboardViewModel
            {
                MyCoursesCount = courses.Count, // Tổng số khóa tham gia

                // Tổng học viên (Tính tất cả các khóa mình có mặt)
                MyStudentsCount = courses.Sum(c => c.EnrollmentCount),

                // Điểm đánh giá trung bình
                AverageRating = courses.Any(c => c.Reviews.Any())
                                ? Math.Round(courses.SelectMany(c => c.Reviews).Average(r => r.Rating), 1)
                                : 0,

                TotalRatingCount = courses.SelectMany(c => c.Reviews).Count(),

                // [FIX] Doanh thu: Chỉ tính cho khóa mình làm chủ (Tránh cộng tiền của người khác vào ví mình trên giao diện)
                TotalRevenue = myOwnedCourses.Sum(c => c.EnrollmentCount * (c.Price > 0 ? c.Price : 0)),

                DisplayCourses = courses.Select(c => new CourseViewModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Thumbnail = c.Thumbnail,
                    CategoryName = c.Category?.CategoryName ?? "Chưa phân loại",
                    Level = c.Level,
                    InstructorName = c.Instructor.FullName, // Hiển thị tên giảng viên chính

                    StudentCount = c.EnrollmentCount,

                    // Tính số bài học an toàn (do đã Include ở trên)
                    LessonCount = c.Chapters?.Sum(ch => ch.Lessons?.Count ?? 0) ?? 0,

                    Rating = c.Reviews.Any() ? Math.Round(c.Reviews.Average(r => r.Rating), 1) : 0,
                    ReviewCount = c.Reviews.Count,

                    OriginalPrice = c.Price,
                    IsPaid = c.Price > 0,
                    DiscountPercent = c.DiscountPercent,

                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdateAt ?? c.CreatedAt,
                    IsPublished = c.IsPublished,

                    // [QUAN TRỌNG] Xác định quyền chủ sở hữu
                    IsOwner = c.InstructorId == userId
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddContent(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Chapters).ThenInclude(ch => ch.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return NotFound();

            var viewModel = new AddContentViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Sections = course.Chapters.OrderBy(c => c.OrderIndex).Select(c => new SectionViewModel
                {
                    ChapterId = c.ChapterId,
                    SectionTitle = c.ChapterName,
                    OrderIndex = c.OrderIndex,
                    Lessons = c.Lessons.OrderBy(l => l.OrderIndex).Select(l => new LessonViewModel
                    {
                        LessonId = l.LessonId,
                        LessonTitle = l.LessonName,
                        VideoDuration = l.DurationInMinutes,
                        IsFree = l.IsFree,
                        VideoUrl = l.VideoUrl,
                        OrderIndex = l.OrderIndex,
                        // Map dữ liệu Meeting
                        LessonType = l.LessonType,
                        StartTime = l.StartTime,
                        MeetingLink = l.MeetingLink
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddContent(int CourseId, string action)
        {
            // Vì nội dung (Chương/Bài học) đã được lưu bằng AJAX tức thì,
            // nên hàm này chủ yếu dùng để điều hướng trang.

            if (action == "next")
            {
                // Chuyển sang trang Xuất bản
                return RedirectToAction("PublishCourse", new { id = CourseId });
            }

            if (action == "draft")
            {
                // Quay về danh sách khóa học
                return RedirectToAction("MyCourses");
            }

            // Mặc định reload lại trang hiện tại
            return RedirectToAction("AddContent", new { id = CourseId });
        }
    
        [HttpPost]
        public async Task<IActionResult> CreateChapter([FromBody] SectionViewModel model)
        {
            try
            {

                if (model.CourseId <= 0)
                    return Json(new { success = false, message = "Không tìm thấy ID khóa học" });
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (!await _authService.AuthorizeAsync(userId, model.CourseId, CoursePermission.ManageContent))
                {
                    return Json(new { success = false, message = "Bạn không có quyền thêm chương cho khóa học này." });
                }
                var maxOrder = await _context.Chapters
                    .Where(c => c.CourseId == model.CourseId)
                    .MaxAsync(c => (int?)c.OrderIndex) ?? 0;

                var chapter = new Chapter
                {
                    ChapterName = model.SectionTitle,
                    CourseId = model.CourseId,
                    OrderIndex = maxOrder + 1,
                    CreateAt = DateTime.Now
                };

                _context.Chapters.Add(chapter);
                await _context.SaveChangesAsync();

                return Json(new { success = true, chapterId = chapter.ChapterId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
      
   
     
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> CreateLesson(
    int chapterId,
    string title,
    int duration,
    bool isFree,
    string videoType,
    string youtubeUrl,
    IFormFile? videoFile,
    IFormFile? documentFile,
    string lessonType,
    DateTime? startTime,
    string? meetingLink,
    string? content)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Lỗi: Vui lòng nhập Tiêu đề bài học.");
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var courseId = await _context.Chapters
            .Where(c => c.ChapterId == chapterId)
            .Select(c => c.CourseId)
            .FirstOrDefaultAsync();
            if (courseId == 0)
            {
                return Json(new { success = false, message = "Không tìm thấy chương học (Chapter) hợp lệ." });
            }
            bool isAuthorized = await _authService.AuthorizeAsync(userId, courseId, CoursePermission.ManageContent);
            if (!isAuthorized)
            {
              
                return Json(new { success = false, message = "Bạn không có quyền sửa đổi nội dung khóa học này." });
            }

            string finalType = "Video";
            if (!string.IsNullOrEmpty(lessonType))
            {
                if (lessonType.Equals("Assignment", StringComparison.OrdinalIgnoreCase))
                {
                    finalType = "Assignment";
                }
                else if (lessonType.Equals("Meeting", StringComparison.OrdinalIgnoreCase) ||
                         lessonType.Equals("Stream", StringComparison.OrdinalIgnoreCase))
                {
                    finalType = "Meeting";
                }
            }

            string videoPath = "";
            string documentPath = "";

            try
            {
              
                if (finalType == "Video")
                {
                    if (videoType == "upload" && videoFile != null && videoFile.Length > 0)
                    {
                        string folder = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
                        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + videoFile.FileName;
                        string filePath = Path.Combine(folder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await videoFile.CopyToAsync(stream);
                        }
                        videoPath = "/videos/" + uniqueFileName;
                    }
                    else if (videoType == "youtube")
                    {
                        videoPath = youtubeUrl;
                    }
                }

                if (documentFile != null && documentFile.Length > 0)
                {
                    string folder = Path.Combine(_webHostEnvironment.WebRootPath, "documents");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + documentFile.FileName;
                    string filePath = Path.Combine(folder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentFile.CopyToAsync(stream);
                    }
                    documentPath = "/documents/" + uniqueFileName;
                }

                var maxOrder = await _context.Lessons
                    .Where(l => l.ChapterId == chapterId)
                    .MaxAsync(l => (int?)l.OrderIndex) ?? 0;

                var lesson = new Lesson
                {
                    ChapterId = chapterId,
                    LessonName = title,
                    LessonType = finalType,
                    DurationInMinutes = duration,
                    IsFree = isFree,
                    Content = content,
                    DocumentUrl = documentPath,
                    CreateAt = DateTime.Now,
                    OrderIndex = maxOrder + 1,
                    VideoUrl = (finalType == "Video") ? videoPath : null,
                    StartTime = (finalType == "Meeting") ? startTime : null,
                    MeetingLink = (finalType == "Meeting") ? meetingLink : null
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync(); 

            
               

                if (courseId > 0)
                {
                    
                    var studentIds = await _context.Enrollments
                        .Where(e => e.CourseId == courseId)
                        .Select(e => e.StudentId)
                        .ToListAsync();

                  
                    var notifications = new List<Notification>();
                    foreach (var studentId in studentIds)
                    {
                        notifications.Add(new Notification
                        {
                            UserId = studentId,
                            Title = "Bài học mới",
                            Message = $"Khóa học vừa cập nhật bài mới: {lesson.LessonName}",
                            Type = NotificationType.Info, 
                            Category = "Course",          
                            RedirectUrl = $"/Learning/{lesson.LessonId}", 
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        });
                    }

                    if (notifications.Any())
                    {
                        _context.Notifications.AddRange(notifications);
                        await _context.SaveChangesAsync();
                    }
                }
            

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Lỗi xử lý: " + ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> PublishCourse(int id)
        {
          
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Chapters)
                .ThenInclude(ch => ch.Lessons)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null) return NotFound();

         
            int totalLessons = course.Chapters.SelectMany(c => c.Lessons).Count();
            int totalDuration = course.Chapters.SelectMany(c => c.Lessons).Sum(l => l.DurationInMinutes);
            int freePreviewCount = course.Chapters.SelectMany(c => c.Lessons).Count(l => l.IsFree);

            var viewModel = new PublishCourseViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                ShortDescription = course.ShortDescription,
            
                CourseImageUrl = string.IsNullOrEmpty(course.Thumbnail)
                                ? "/images/default-course.jpg"
                                : $"/uploads/{course.Thumbnail}",
                CategoryName = course.Category?.CategoryName ?? "Chưa phân loại",
                Level = course.Level,
                Price = course.Price,
               
                DiscountPrice = course.DiscountPercent.HasValue
                                ? course.Price * (100 - course.DiscountPercent.Value) / 100
                                : null,
                CourseType = (course.Price == 0) ? "Free" : "Paid",

                TotalSections = course.Chapters.Count,
                TotalLessons = totalLessons,
                TotalDuration = totalDuration,
                FreePreviewCount = freePreviewCount
            };

            return View(viewModel);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishCourse(int CourseId, string action)
        {
            var course = await _context.Courses.FindAsync(CourseId);
            if (course == null) return NotFound();

            if (action == "publish")
            {
               

                course.IsPublished = true;
                course.UpdateAt = DateTime.Now; 

                await _context.SaveChangesAsync();

            
                TempData["PublishSuccess"] = true;

               
                return RedirectToAction("PublishCourse", new { id = CourseId });
            }
            else if (action == "draft")
            {
            
                return RedirectToAction("MyCourses");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Students(int? courseId, string search)
        {
           
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int instructorId = int.Parse(userIdStr);

     
            var myCourses = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new CourseCompactViewModel { CourseId = c.CourseId, CourseName = c.CourseName })
                .ToListAsync();

           
            var query = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Include(e => e.LessonProgresses)
                .Where(e => e.Course.InstructorId == instructorId);

          
            if (courseId.HasValue && courseId.Value > 0)
            {
                query = query.Where(e => e.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(e => e.Student.FullName.ToLower().Contains(search) ||
                                         e.Student.Email.ToLower().Contains(search));
            }

            var enrollments = await query.OrderByDescending(e => e.EnrollmentAt).ToListAsync();

        
            var studentList = enrollments.Select(e => new StudentEnrolledViewModel
            {
                StudentId = e.StudentId,
                StudentName = e.Student.FullName,
                StudentEmail = e.Student.Email,
                PhoneNumber = e.Student.PhoneNumber,
                CourseId = e.CourseId,
                CourseName = e.Course.CourseName,
                EnrollmentDate = e.EnrollmentAt,
                ProgressPercent = e.ProgressPersent,
                LastActive = e.LessonProgresses.OrderByDescending(lp => lp.LastAccessedAt).FirstOrDefault()?.LastAccessedAt
            }).ToList();

          
            var viewModel = new InstructorStudentsViewModel
            {
                Courses = myCourses ?? new List<CourseCompactViewModel>(),
                SelectedCourseId = courseId,
                Students = studentList ?? new List<StudentEnrolledViewModel>()
            };

          
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Analytics()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 1. Lấy dữ liệu thô từ DB
            var courseIds = await _context.Courses.Where(c => c.InstructorId == userId).Select(c => c.CourseId).ToListAsync();

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => courseIds.Contains(e.CourseId))
                .OrderByDescending(e => e.EnrollmentAt)
                .ToListAsync();

            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => courseIds.Contains(r.CourseId))
                .OrderByDescending(r => r.CreateAt)
                .ToListAsync();

            // 2. Tính toán Card thống kê
            var now = DateTime.Now;
            var startOfThisMonth = new DateTime(now.Year, now.Month, 1);
            var startOfLastMonth = startOfThisMonth.AddMonths(-1);

            decimal totalRevenue = enrollments.Sum(e => e.PaidAmount);
            decimal thisMonthRev = enrollments.Where(e => e.EnrollmentAt >= startOfThisMonth).Sum(e => e.PaidAmount);
            decimal lastMonthRev = enrollments.Where(e => e.EnrollmentAt >= startOfLastMonth && e.EnrollmentAt < startOfThisMonth).Sum(e => e.PaidAmount);

            double growth = 0;
            if (lastMonthRev > 0) growth = (double)((thisMonthRev - lastMonthRev) / lastMonthRev * 100);
            else if (thisMonthRev > 0) growth = 100;

            // 3. Dữ liệu Biểu đồ (4 tuần gần nhất)
            var chartLabels = new List<string>();
            var revenueData = new List<decimal>();
            // Logic chia 4 tuần (đơn giản hóa)
            for (int i = 3; i >= 0; i--)
            {
                var weekStart = now.AddDays(-i * 7 - 7);
                var weekEnd = now.AddDays(-i * 7);
                chartLabels.Add($"Tuần {4 - i}");
                revenueData.Add(enrollments.Where(e => e.EnrollmentAt >= weekStart && e.EnrollmentAt <= weekEnd).Sum(e => e.PaidAmount));
            }

            // 4. Top Khóa học
            string[] colors = { "#059669", "#2563eb", "#f59e0b", "#8b5cf6", "#ec4899" };
            int totalStudents = enrollments.Count;

            var topCourses = enrollments
                .GroupBy(e => e.Course.CourseName)
                .Select((g, index) => new CoursePerformance
                {
                    CourseName = g.Key,
                    StudentCount = g.Count(),
                    Revenue = g.Sum(e => e.PaidAmount),
                    Percentage = totalStudents > 0 ? Math.Round((double)g.Count() / totalStudents * 100, 1) : 0,
                    ColorCode = colors[index % colors.Length]
                })
                .OrderByDescending(x => x.StudentCount)
                .Take(5)
                .ToList();

            // 5. Hoạt động gần đây (Gộp Enrollments và Reviews)
            var activities = new List<RecentActivity>();

            // Thêm 5 học viên mới nhất
            activities.AddRange(enrollments.Take(5).Select(e => new RecentActivity
            {
                Type = "NewStudent",
                Title = "Học viên mới",
                Description = $"<b>{e.Student.FullName}</b> đã đăng ký khóa <b>{e.Course.CourseName}</b>",
                TimeDisplay = (DateTime.Now - e.EnrollmentAt).TotalHours < 24
                              ? $"{(int)(DateTime.Now - e.EnrollmentAt).TotalHours} giờ trước"
                              : e.EnrollmentAt.ToString("dd/MM")
            }));

            // Thêm 5 review mới nhất
            activities.AddRange(reviews.Take(5).Select(r => new RecentActivity
            {
                Type = "Review",
                Title = $"Đánh giá {r.Rating} sao",
                Description = $"<b>{r.Student.FullName}</b>: \"{r.Comment}\"",
                TimeDisplay = (DateTime.Now - r.CreateAt).TotalHours < 24
                              ? $"{(int)(DateTime.Now - r.CreateAt).TotalHours} giờ trước"
                              : r.CreateAt.ToString("dd/MM")
            }));

            // 6. Đóng gói ViewModel
            var viewModel = new InstructorAnalyticsViewModel
            {
                TotalRevenue = totalRevenue,
                MonthlyRevenue = thisMonthRev,
                RevenueGrowth = Math.Round(growth, 1),
                NewStudentsMonth = enrollments.Count(e => e.EnrollmentAt >= startOfThisMonth),
                AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 1) : 0,
                TotalRatingCount = reviews.Count,
                TotalStudents = totalStudents,
                ChartLabels = chartLabels,
                RevenueData = revenueData,
                TopCourses = topCourses,
                Activities = activities.OrderByDescending(x => x.TimeDisplay.Contains("giờ")).Take(6).ToList() // Sắp xếp sơ bộ
            };

            return View(viewModel);
        }
     
       
        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

           
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id && c.InstructorId == userId);

            if (course == null) return NotFound();

         
            var model = new CreateCourseViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                ShortDescription = course.ShortDescription,
                CategoryId = course.CategoryId,
                Level = course.Level,
                Language = course.Language,
                Price = course.Price,
                DiscountPrice = course.DiscountPercent.HasValue ? course.Price * (100 - course.DiscountPercent.Value) / 100 : null,
                EstimatedDuration = course.DurationInHours,
                Requirements = course.Requirements,
                WhatYouWillLearn = course.WhatYouWillLearn,
                CurrentThumbnail = course.Thumbnail,
                CourseType = course.Price > 0 ? "Paid" : "Free",

               
                Categories = await _context.Categories.Where(c => c.IsActive).ToListAsync()
            };

            return View("CreateCourse", model); 
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCourse(CreateCourseViewModel model, string action) 
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!model.CourseId.HasValue)
            {
                return BadRequest("CourseId is required.");
            }
            if (!await _authService.AuthorizeAsync(userId, model.CourseId.Value, CoursePermission.ManageSettings))
            {
                return Forbid(); 
            }

            var course = await _context.Courses.FindAsync(model.CourseId);

            if (course == null) return NotFound();

            if (ModelState.IsValid)
            {
                if (model.CourseImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CourseImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CourseImage.CopyToAsync(fileStream);
                    }
                    course.Thumbnail = uniqueFileName; 
                }

                course.CourseName = model.CourseName;
                course.ShortDescription = model.ShortDescription;
                course.CategoryId = model.CategoryId;
                course.Level = model.Level;
                course.Language = model.Language;
                course.Requirements = model.Requirements;
                course.WhatYouWillLearn = model.WhatYouWillLearn;
                course.DurationInHours = (int)model.EstimatedDuration;
                course.UpdateAt = DateTime.Now;

                if (model.CourseType == "Paid" && model.Price.HasValue)
                {
                    course.Price = model.Price.Value;
                    if (model.DiscountPrice.HasValue && model.DiscountPrice.Value < course.Price)
                    {
                        course.DiscountPercent = (int)((1 - (model.DiscountPrice.Value / course.Price)) * 100);
                    }
                    else
                    {
                        course.DiscountPercent = null;
                    }
                }
                else
                {
                    course.Price = 0;
                    course.DiscountPercent = null;
                }

                await _context.SaveChangesAsync();

           
                if (action == "next")
                {
                   
                    return RedirectToAction("AddContent", new { id = course.CourseId });
                }

          
                TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                return RedirectToAction("MyCourses");
            }

       
            model.Categories = await _context.Categories.ToListAsync();
            return View("CreateCourse", model);
        }
      
        [HttpPost]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!await _authService.AuthorizeAsync(userId, id, CoursePermission.ManageSettings))
            {
                return Json(new { success = false, message = "Bạn không có quyền thay đổi trạng thái khóa học này." });
            }
            var course = await _context.Courses.FindAsync(id);

            if (course == null) return Json(new { success = false, message = "Không tìm thấy khóa học" });


            course.IsPublished = !course.IsPublished;

       
            if (course.IsPublished) course.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true, isPublished = course.IsPublished });
        }
     
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!await _authService.AuthorizeAsync(userId, id, CoursePermission.ManageSettings))
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa khóa học này." });
            }

            var course = await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.Chapters).ThenInclude(ch => ch.Lessons) 
                .FirstOrDefaultAsync(c => c.CourseId == id && c.InstructorId == userId);
            if (course == null) return Json(new { success = false, message = "Không tìm thấy khóa học." });
           
            if (course.Enrollments.Any())
            {
                return Json(new
                {
                    success = false,
                    message = $"Khóa học này đang có {course.Enrollments.Count} học viên. Bạn không thể xóa, hãy chọn 'Ẩn khóa học' thay thế."
                });
            }

            try
            {
             
                if (!string.IsNullOrEmpty(course.Thumbnail))
                {
                    string thumbPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", course.Thumbnail);
                    if (System.IO.File.Exists(thumbPath)) System.IO.File.Delete(thumbPath);
                }

             
                foreach (var chapter in course.Chapters)
                {
                    foreach (var lesson in chapter.Lessons)
                    {
                     
                        if (!string.IsNullOrEmpty(lesson.VideoUrl) && lesson.VideoUrl.StartsWith("/videos/"))
                        {
                            string videoPath = Path.Combine(_webHostEnvironment.WebRootPath, lesson.VideoUrl.TrimStart('/'));
                            if (System.IO.File.Exists(videoPath)) System.IO.File.Delete(videoPath);
                        }
                    
                        if (!string.IsNullOrEmpty(lesson.DocumentUrl))
                        {
                            string docPath = Path.Combine(_webHostEnvironment.WebRootPath, lesson.DocumentUrl.TrimStart('/'));
                            if (System.IO.File.Exists(docPath)) System.IO.File.Delete(docPath);
                        }
                    }
                }

             
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã xóa khóa học vĩnh viễn." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
   
        [HttpPost]
        public async Task<IActionResult> UpdateLesson(int lessonId, string title, int duration, bool isFree, IFormFile? videoFile, IFormFile? documentFile, string lessonType, DateTime? startTime, string? meetingLink, string? content)
        {
            var lesson = await _context.Lessons
         .Include(l => l.Chapter) 
         .FirstOrDefaultAsync(l => l.LessonId == lessonId);
            if (lesson == null) return Json(new { success = false, message = "Không tìm thấy bài học." });
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!await _authService.AuthorizeAsync(userId, lesson.Chapter.CourseId, CoursePermission.ManageContent))
            {
                return Json(new { success = false, message = "Không có quyền sửa bài học này." });
            }
            lesson.LessonName = title;
            lesson.DurationInMinutes = duration;
            lesson.IsFree = isFree;
            lesson.Content = content;

         
            lesson.LessonType = (lessonType == "meeting" || lessonType == "stream") ? "Meeting" : "Video";
            lesson.StartTime = (lessonType == "meeting" || lessonType == "stream") ? startTime : null;
            lesson.MeetingLink = (lessonType == "meeting" || lessonType == "stream") ? meetingLink : null;

           
            if (videoFile != null && lessonType != "meeting" && lessonType != "stream")
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
                string fileName = Guid.NewGuid() + "_" + videoFile.FileName;
                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }
                lesson.VideoUrl = "/videos/" + fileName;
            }

         
            if (documentFile != null)
            {
                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "documents");
                string fileName = Guid.NewGuid() + "_" + documentFile.FileName;
                using (var stream = new FileStream(Path.Combine(folder, fileName), FileMode.Create))
                {
                    await documentFile.CopyToAsync(stream);
                }
                lesson.DocumentUrl = "/documents/" + fileName;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons
          .Include(l => l.Chapter)
          .FirstOrDefaultAsync(l => l.LessonId == id);
            if (lesson == null) return Json(new { success = false, message = "Không tìm thấy bài học" });
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            
            if (!await _authService.AuthorizeAsync(userId, lesson.Chapter.CourseId, CoursePermission.ManageContent))
            {
                return Json(new { success = false, message = "Bạn không có quyền xóa bài học này." });
            }
            try
            {
              
                if (!string.IsNullOrEmpty(lesson.VideoUrl) && lesson.VideoUrl.StartsWith("/videos/"))
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, lesson.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
                if (!string.IsNullOrEmpty(lesson.DocumentUrl) && lesson.DocumentUrl.StartsWith("/documents/"))
                {
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, lesson.DocumentUrl.TrimStart('/'));
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }

               
                var relatedProgress = _context.LessonProgresses.Where(lp => lp.LessonId == id);
                _context.LessonProgresses.RemoveRange(relatedProgress);

              
                _context.Lessons.Remove(lesson);

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xóa: " + ex.Message });
            }
        }
     
        public async Task<IActionResult> TeachingSchedule()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Account");
            int userId = int.Parse(userIdStr);

            var viewModel = new TeachingScheduleViewModel();

            var myCourses = await _context.Courses
                .Include(c => c.Enrollments)
                .Include(c => c.Chapters).ThenInclude(ch => ch.Lessons).ThenInclude(l => l.LessonProgresses)
                .Include(c => c.Collaborators) 
                .Where(c =>
                    c.InstructorId == userId || 
                    c.Collaborators.Any(cb => cb.CollaboratorId == userId && cb.Status == "Accepted") 
                )
                .ToListAsync();
            int instructorId = int.Parse(userIdStr);

           

       
         

            viewModel.MyCourses = myCourses.Select(c => new CourseCompactViewModel
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName
            }).ToList();

           
            foreach (var course in myCourses)
            {
                var lessons = course.Chapters.SelectMany(c => c.Lessons);

                foreach (var lesson in lessons)
                {
                   
                    if (lesson.LessonType == "Meeting" || lesson.LessonType == "Stream")
                    {
                      
                        if (lesson.StartTime.HasValue)
                        {
                            viewModel.ScheduledSessions.Add(new TeachingSessionItem
                            {
                                CourseId = course.CourseId,
                                LessonId = lesson.LessonId,
                                LessonName = lesson.LessonName,
                                CourseName = course.CourseName,
                                StartTime = lesson.StartTime.Value,
                                DurationMinutes = lesson.DurationInMinutes,
                                StudentCount = course.Enrollments.Count,
                                MeetingLink = lesson.MeetingLink,
                                DocumentUrl = lesson.DocumentUrl
                            });
                        }
                    }

               
                    if (lesson.LessonType == "Assignment" || lesson.DueDate.HasValue)
                    {
                        int totalStudents = course.Enrollments.Count;
                      
                        int submitted = lesson.LessonProgresses.Count(p => p.SubmittedAt.HasValue || !string.IsNullOrEmpty(p.SubmissionUrl));
                     
                        int graded = lesson.LessonProgresses.Count(p => p.Score.HasValue);

                        viewModel.PendingGrading.Add(new GradingItem
                        {
                            LessonId = lesson.LessonId,
                            AssignmentTitle = lesson.LessonName,
                            AssignmentContent = lesson.Content,
                            CourseName = course.CourseName,
                            DueDate = lesson.DueDate,
                            TotalStudents = totalStudents,
                            SubmittedCount = submitted,
                            GradedCount = graded
                        });
                    }
                }
            }

         
            viewModel.ScheduledSessions = viewModel.ScheduledSessions.OrderBy(s => s.StartTime).ToList();

          
            viewModel.PendingGrading = viewModel.PendingGrading
                .OrderByDescending(g => g.IsOverdue)
                .ThenByDescending(g => g.PendingCount)
                .ToList();

            return View(viewModel);
        }
     
      
        [HttpPost]
        public async Task<IActionResult> AutoGenerateAssignmentDesc(string title)
        {
            try
            {
              
                var content = await _geminiService.GenerateAssignmentContentAsync(title);
                return Json(new { success = true, data = content });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi Server: " + ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetChapters(int courseId)
        {
            var chapters = await _context.Chapters
                .Where(c => c.CourseId == courseId)
                .OrderBy(c => c.OrderIndex)
                .Select(c => new { c.ChapterId, c.ChapterName })
                .ToListAsync();

            return Json(chapters);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubmissions(int lessonId)
        {
            var submissions = await _context.LessonProgresses
                .Include(lp => lp.Enrollment).ThenInclude(e => e.Student)
                .Where(lp => lp.LessonId == lessonId && (lp.SubmittedAt != null || lp.SubmissionUrl != null))
                .OrderByDescending(lp => lp.SubmittedAt)
                .Select(lp => new
                {
                    progressId = lp.ProgressId,
                    studentName = lp.Enrollment.Student.FullName,
                    studentAvatar = lp.Enrollment.Student.Avatar,
                    submittedAt = lp.SubmittedAt.HasValue ? lp.SubmittedAt.Value.ToString("dd/MM/yyyy HH:mm") : "N/A",
                    submissionUrl = lp.SubmissionUrl,
                    score = lp.Score,
                    feedback = lp.InstructorFeedback,
                    isText = lp.SubmissionUrl != null && lp.SubmissionUrl.EndsWith(".txt") 
                })
                .ToListAsync();

            var result = submissions.Select(s => {
                string contentPreview = null;
                if (s.isText && !string.IsNullOrEmpty(s.submissionUrl))
                {
                    try
                    {
                        // Đường dẫn vật lý
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, s.submissionUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            contentPreview = System.IO.File.ReadAllText(filePath);
                        }
                    }
                    catch { contentPreview = "Lỗi đọc nội dung."; }
                }
                return new { s.progressId, s.studentName, s.studentAvatar, s.submittedAt, s.submissionUrl, s.score, s.feedback, s.isText, contentPreview };
            });

            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> GradeSubmission(int progressId, int score, string feedback)
        {
            
            var progress = await _context.LessonProgresses
           .Include(p => p.Lesson)
           .Include(p => p.Enrollment) 
           .FirstOrDefaultAsync(p => p.ProgressId == progressId);

            if (progress == null) return Json(new { success = false, message = "Không tìm thấy bài nộp" });
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!await _authService.AuthorizeAsync(userId, progress.Enrollment.CourseId, CoursePermission.Grade))
            {
                return Json(new { success = false, message = "Bạn không có quyền chấm điểm khóa học này." });
            }
            progress.Score = score;
            progress.InstructorFeedback = feedback;
            progress.IsCompleted = true;

            // 3. THÊM MỚI: Tạo thông báo gửi cho học viên
            // Lấy StudentId từ Enrollment
            // Lấy LessonName từ Lesson
            await CreateNotification(
                userId: progress.Enrollment.StudentId,
                title: "Bài tập đã được chấm",
                message: $"Bài '{progress.Lesson.LessonName}' đạt {score} điểm. GV nhận xét: {feedback}",
                type: NotificationType.Success, // Màu xanh lá
                category: "Grade",              // Icon Huy hiệu/Điểm
                url: $"/Courses/Details/{progress.Enrollment.CourseId}" // Link về khóa học
            );

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int instructorId = int.Parse(userIdStr);

            var reviews = await _context.Reviews
                .Include(r => r.Course)
                .Include(r => r.Student)
                .Where(r => r.Course.InstructorId == instructorId)
                .OrderByDescending(r => r.CreateAt)
                .ToListAsync();

            return View(reviews);
        }
     
        [HttpGet]
        public async Task<IActionResult> GetCourseReviews(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId && c.InstructorId == userId);
            if (course == null) return Json(new { success = false, message = "Không tìm thấy khóa học hoặc bạn không có quyền." });

            var reviews = await _context.Reviews
                .Include(r => r.Student)
                .Where(r => r.CourseId == courseId)
                .OrderByDescending(r => r.CreateAt)
                .Select(r => new
                {
                    reviewId = r.ReviewId,
                    studentName = r.Student.FullName,
                    avatar = r.Student.Avatar, 
                    rating = r.Rating,
                    comment = r.Comment,
                    date = r.CreateAt.ToString("dd/MM/yyyy"),
                    reply = r.InstructorReply,
                    replyAt = r.ReplyAt.HasValue ? r.ReplyAt.Value.ToString("dd/MM/yyyy") : ""
                })
                .ToListAsync();

            return Json(new { success = true, data = reviews });
        }

  
        [HttpPost]
        public async Task<IActionResult> ReplyToReview(int reviewId, string replyContent)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var review = await _context.Reviews
                .Include(r => r.Course)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null) return Json(new { success = false, message = "Không tìm thấy đánh giá." });

        
            if (review.Course.InstructorId != userId)
                return Json(new { success = false, message = "Bạn không có quyền phản hồi đánh giá này." });

            review.InstructorReply = replyContent;
            review.ReplyAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        public async Task<IActionResult> ManageEnrollments()
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var requests = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e => e.Course.InstructorId == instructorId && e.Status == "Pending")
                .ToListAsync();
            return View(requests);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrReject(int enrollmentId, string action, string? reason)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var enrollment = await _context.Enrollments
                    .Include(e => e.Course).ThenInclude(c => c.Instructor)
                    .Include(e => e.Student)
                    .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);

                if (action == "Approve")
                {
                 
                    enrollment.Status = "Approved";
                    decimal instructorNet = enrollment.PaidAmount * 0.9m;
                    enrollment.Course.Instructor.Balance += instructorNet;

                    _context.Wallet.Add(new Wallet
                    {
                        UserId = enrollment.Course.InstructorId,
                        Amount = instructorNet,
                        Type = "Revenue",
                        Description = $"Thu nhập từ học viên: {enrollment.Student.FullName}",
                        Status = "Completed",
                        CreatedAt = DateTime.Now
                    });
                    enrollment.Course.EnrollmentCount += 1;

                    // Gửi thông báo cho học viên khi được duyệt
                    await CreateNotification(
                        userId: enrollment.StudentId,
                        title: "Đăng ký khóa học được duyệt",
                        message: $"Chúc mừng! Bạn đã được chấp nhận tham gia khóa học '{enrollment.Course.CourseName}'.",
                        type: NotificationType.Success,
                        category: "Course",
                        url: $"/Courses/Learn?id={enrollment.CourseId}"
                    );
                }
                else
                {
                 
                    enrollment.Status = "Rejected";
                    enrollment.RejectionReason = reason;
                    enrollment.Student.Balance += enrollment.PaidAmount;

                    _context.Wallet.Add(new Wallet
                    {
                        UserId = enrollment.StudentId,
                        Amount = enrollment.PaidAmount,
                        Type = "Refund",
                        Description = $"Hoàn tiền khóa học bị từ chối: {enrollment.Course.CourseName}",
                        Status = "Completed",
                        CreatedAt = DateTime.Now
                    });

                    // Gửi thông báo cho học viên khi bị từ chối
                    await CreateNotification(
                        userId: enrollment.StudentId,
                        title: "Đăng ký khóa học bị từ chối",
                        message: $"Rất tiếc, yêu cầu tham gia khóa học '{enrollment.Course.CourseName}' của bạn đã bị từ chối.{(string.IsNullOrEmpty(reason) ? "" : $" Lý do: {reason}")}",
                        type: NotificationType.Error,
                        category: "Course",
                        url: "/Student/MyCourses"
                    );
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Json(new { success = true });
            }
            catch
            {
                await transaction.RollbackAsync();
                return Json(new { success = false });
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Profile(int? id)
        {
            
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                if (string.IsNullOrEmpty(currentUserId)) return Challenge(); 
                id = int.Parse(currentUserId);
            }

          
            var instructor = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && u.Role == "Instructor");

            if (instructor == null) return NotFound();

         
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Where(c => c.InstructorId == id.Value && c.IsPublished)
                .ToListAsync();

            var viewModel = new InstructorProfileViewModel
            {
                Instructor = instructor,
                Courses = courses,
                TotalStudents = courses.Sum(c => c.EnrollmentCount)
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfileAjax([FromForm] InstructorEditProfileViewModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return Json(new { success = false, message = "Không tìm thấy người dùng." });

            try
            {
              
                user.FullName = model.FullName;
                user.Bio = model.Bio;

                if (model.AvatarFile != null)
                {
                  
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "avatars");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.AvatarFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AvatarFile.CopyToAsync(fileStream);
                    }

                    user.Avatar = uniqueFileName; 
                }

                await _context.SaveChangesAsync();

              
                return Json(new { success = true, message = "Cập nhật hồ sơ thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }
        // Hàm phụ trợ tạo thông báo nhanh
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


