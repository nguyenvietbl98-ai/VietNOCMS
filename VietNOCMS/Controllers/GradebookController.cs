using ClosedXML.Excel; // Thư viện Excel miễn phí
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class GradebookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradebookController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index(int courseId)
        {
           
            var enrollments = await _context.Enrollments
                .Include(e => e.Student) 
                .Include(e => e.Course)
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

          
            var assignments = await _context.Lessons
                .Include(l => l.Chapter)
                .Where(l => l.Chapter.CourseId == courseId && l.LessonType == "Assignment")
                .OrderBy(l => l.OrderIndex) 
                .ToListAsync();

           
            ViewBag.Assignments = assignments;
            ViewBag.CourseId = courseId;
            ViewBag.CourseName = enrollments.FirstOrDefault()?.Course?.CourseName ?? "Khóa học";

            return View(enrollments);
        }

      
        [HttpPost]
        public async Task<IActionResult> CalculateGrades(int courseId)
        {
           
            var enrollments = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

           
            var assignments = await _context.Lessons
                .Include(l => l.Chapter)
                .Where(l => l.Chapter.CourseId == courseId && l.LessonType == "Assignment")
                .ToListAsync();

           
            if (assignments.Count == 0)
            {
                return Json(new { success = false, message = "Khóa học này chưa có bài tập nào để tính điểm." });
            }

            foreach (var enr in enrollments)
            {
               
                var submissions = await _context.LessonProgresses
                    .Include(p => p.Lesson)
                    .Where(p =>
                        p.EnrollmentId == enr.EnrollmentId &&
                        p.Lesson.LessonType == "Assignment"
                    )
                    .ToListAsync();

               
                double totalScore = submissions.Sum(s => s.Score ?? 0);

               
                double finalScore = totalScore / assignments.Count;

              
                enr.FinalScore = Math.Round(finalScore, 1);

             
                if (enr.FinalScore >= 9.0) enr.Rank = "Xuất sắc";
                else if (enr.FinalScore >= 8.0) enr.Rank = "Giỏi";
                else if (enr.FinalScore >= 6.5) enr.Rank = "Khá";
                else if (enr.FinalScore >= 5.0) enr.Rank = "Trung bình";
                else enr.Rank = "Yếu";

                // Cập nhật ngày hoàn thành
                enr.CompletedAt = DateTime.Now;

                // Cập nhật trạng thái chứng chỉ (Ví dụ: >= 5.0 là Đậu)
                enr.IsCertified = enr.FinalScore >= 5.0;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã tính toán và cập nhật điểm số thành công!" });
        }

        
        public async Task<IActionResult> ExportExcel(int courseId)
        {
            // 1. Lấy dữ liệu
            var course = await _context.Courses.FindAsync(courseId);
            var data = await _context.Enrollments
                .Include(e => e.Student) // Include thông tin sinh viên
                .Where(e => e.CourseId == courseId)
                .ToListAsync();

            if (course == null) return NotFound();

            // 2. Khởi tạo File Excel
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("BangDiem");

                // --- Tạo Header ---
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 2).Value = "Họ và Tên";
                worksheet.Cell(1, 3).Value = "Email";
                worksheet.Cell(1, 4).Value = "Điểm Tổng Kết";
                worksheet.Cell(1, 5).Value = "Xếp Loại";
                worksheet.Cell(1, 6).Value = "Trạng Thái";
                worksheet.Cell(1, 7).Value = "Ngày Hoàn Thành";

                // --- Style cho Header (Màu xanh, in đậm, căn giữa) ---
                var headerRange = worksheet.Range("A1:G1");
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // --- Đổ dữ liệu ---
                int row = 2;
                int stt = 1;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = stt++;
                    worksheet.Cell(row, 2).Value = item.Student?.FullName ?? "N/A";
                    worksheet.Cell(row, 3).Value = item.Student?.Email ?? "N/A";

                    // Xử lý hiển thị điểm (nếu chưa có điểm thì hiện gạch ngang)
                    if (item.FinalScore.HasValue)
                        worksheet.Cell(row, 4).Value = item.FinalScore.Value;
                    else
                        worksheet.Cell(row, 4).Value = "--";

                    worksheet.Cell(row, 5).Value = item.Rank ?? "--";

                    // Trạng thái Đậu/Trượt
                    if (item.IsCertified)
                    {
                        worksheet.Cell(row, 6).Value = "Đạt (Pass)";
                        worksheet.Cell(row, 6).Style.Font.FontColor = XLColor.Green;
                    }
                    else
                    {
                        worksheet.Cell(row, 6).Value = "Chưa đạt";
                        worksheet.Cell(row, 6).Style.Font.FontColor = XLColor.Red;
                    }

                    worksheet.Cell(row, 7).Value = item.CompletedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";

                    // Tô đỏ ô xếp loại nếu điểm kém (Dưới 5.0)
                    if (item.FinalScore.HasValue && item.FinalScore < 5.0)
                    {
                        worksheet.Cell(row, 5).Style.Font.FontColor = XLColor.Red;
                        worksheet.Cell(row, 5).Style.Font.Bold = true;
                    }

                    row++;
                }

                // Tự động căn chỉnh độ rộng cột theo nội dung
                worksheet.Columns().AdjustToContents();

                // Xuất file ra MemoryStream để tải về
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    string fileName = $"BangDiem_{RemoveSign4VietnameseString(course.CourseName)}_{DateTime.Now:ddMMyyyy}.xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

      
        private static readonly string[] VietnameseSigns = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string RemoveSign4VietnameseString(string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str.Replace(" ", "_");
        }
        [HttpGet]
        public async Task<IActionResult> MyGrades()
        {
            // 1. Lấy ID user đang đăng nhập
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Lấy danh sách các khóa đã học kèm kết quả
            var myGrades = await _context.Enrollments
                .Include(e => e.Course) // Lấy tên khóa học
                .Where(e => e.StudentId == userId)
                .OrderByDescending(e => e.EnrollmentAt) // Khóa mới nhất lên đầu
                .ToListAsync();

            return View(myGrades);
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseDetails(int courseId)
        {
            // Lấy ID User hiện tại
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại." });
            }

            try
            {
                // 1. Lấy tất cả bài tập của khóa học này
                var assignments = await _context.Lessons
                    .Include(l => l.Chapter)
                    .Where(l => l.Chapter.CourseId == courseId && l.LessonType == "Assignment")
                    .OrderBy(l => l.OrderIndex)
                    .Select(l => new { l.LessonId, l.LessonName }) // Chỉ lấy trường cần thiết
                    .ToListAsync();

                // 2. Lấy tiến độ làm bài của học viên trong khóa này
                var progress = await _context.LessonProgresses
                    .Include(p => p.Enrollment)
                    .Where(p => p.Enrollment.StudentId == userId && p.Enrollment.CourseId == courseId)
                    .ToListAsync();

                // 3. Ghép dữ liệu (Left Join logic)
                var result = assignments.Select(a => {
                    var p = progress.FirstOrDefault(x => x.LessonId == a.LessonId);
                    return new
                    {
                        lessonName = a.LessonName,
                        score = p?.Score, // null nếu chưa có điểm
                        submittedAt = p?.SubmittedAt?.ToString("dd/MM/yyyy HH:mm") ?? "--",
                        feedback = p?.InstructorFeedback ?? "",
                        // Xác định trạng thái
                        status = p?.Score != null ? "graded" : (p?.SubmittedAt != null ? "pending" : "not_started")
                    };
                });

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
    }
}