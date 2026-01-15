using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VietNOCMS.Data;
using VietNOCMS.Services;

namespace VietNOCMS.Controllers
{
  
    public class AiStudentController : Controller
    {
        private readonly GeminiService _geminiService;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AiStudentController(GeminiService geminiService, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _geminiService = geminiService;
            _context = context;
            _env = env;
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> SummarizeCurrentDocument(int lessonId)
        {
            try
            {
                var lesson = await _context.Lessons.FindAsync(lessonId);
                if (lesson == null) return Json(new { success = false, message = "Không tìm thấy bài học." });

                if (string.IsNullOrEmpty(lesson.DocumentUrl))
                {
                    return Json(new { success = false, message = "Bài học này chưa có tài liệu đính kèm để tóm tắt." });
                }

              
                string relativePath = lesson.DocumentUrl.TrimStart('/', '\\');
                string fullPath = Path.Combine(_env.WebRootPath, relativePath);

                if (!System.IO.File.Exists(fullPath))
                {
                    return Json(new { success = false, message = $"File không tồn tại trên máy chủ. (Path: {lesson.DocumentUrl})" });
                }

                ////////////////////////////CHuyển dạng sang text/////////////////////////////
                string docContent = VietNOCMS.Services.DocumentParser.ParseLocalFile(fullPath);

              
                var summary = await _geminiService.SummarizeDocumentContentAsync(docContent);

                return Json(new { success = true, summary = summary });
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error SummarizeCurrentDocument: {ex.Message}");
                return Json(new { success = false, message = "Lỗi xử lý: " + ex.Message });
            }
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AskTutor(int lessonId, string question)
        {
            try
            {
                if (string.IsNullOrEmpty(question)) return Json(new { success = false, message = "Bạn chưa nhập câu hỏi." });

                var lesson = await _context.Lessons.FindAsync(lessonId);
                if (lesson == null) return Json(new { success = false, message = "Bài học không tồn tại." });

              
                string content = !string.IsNullOrEmpty(lesson.Content) ? lesson.Content : $"Bài học: {lesson.LessonName}. (Hiện chưa có nội dung chi tiết)";

                var answer = await _geminiService.ChatWithLessonAsync(content, question);
                return Json(new { success = true, answer });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error AskTutor: {ex.Message}");
                return Json(new { success = false, message = "Lỗi AI: " + ex.Message });
            }
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summarize(int lessonId)
        {
            try
            {
                var lesson = await _context.Lessons.FindAsync(lessonId);
                if (lesson == null) return NotFound();

                string content = lesson.Content ?? "";
                if (content.Length < 50) return Json(new { success = false, message = "Nội dung quá ngắn để tóm tắt." });

                var summary = await _geminiService.SummarizeLessonAsync(content);
                return Json(new { success = true, summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewAssignment(int lessonId, string submission, IFormFile? file)
        {
            try
            {
              
                var lesson = await _context.Lessons
                    .Include(l => l.Chapter).ThenInclude(c => c.Course).ThenInclude(co => co.Category)
                    .FirstOrDefaultAsync(l => l.LessonId == lessonId);

                if (lesson == null) return Json(new { success = false, message = "Bài học không tồn tại." });

              
                string fullContentToCheck = submission ?? "";

             
                if (file != null && file.Length > 0)
                {
                    try
                    {
                        string fileContent = await VietNOCMS.Services.DocumentParser.ParseFileAsync(file);
                        fullContentToCheck += $"\n\n--- NỘI DUNG TỪ FILE ({file.FileName}) ---\n{fileContent}";
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = "Lỗi đọc file đính kèm: " + ex.Message });
                    }
                }

                if (string.IsNullOrWhiteSpace(fullContentToCheck))
                {
                    return Json(new { success = false, message = "Vui lòng nhập nội dung hoặc đính kèm file để chấm." });
                }

               
                string subject = lesson.Chapter.Course.Category?.CategoryName ?? "Chung";
                string question = !string.IsNullOrEmpty(lesson.Content) ? lesson.Content : lesson.LessonName;

                var review = await _geminiService.ReviewAssignmentAsync(subject, question, fullContentToCheck);
                return Json(new { success = true, review });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ReviewAssignment: {ex.Message}");
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummarizeUploadedFile(IFormFile file)
        {
            if (file == null) return Json(new { success = false, message = "Chưa chọn file!" });

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf" && ext != ".docx" && ext != ".txt")
            {
                return Json(new { success = false, message = "Chỉ hỗ trợ file .PDF, .DOCX, .TXT" });
            }

            try
            {
                string extractedText = await VietNOCMS.Services.DocumentParser.ParseFileAsync(file);

                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    return Json(new { success = false, message = "Không đọc được nội dung file (Có thể là file ảnh/scan)." });
                }

                var summary = await _geminiService.SummarizeDocumentContentAsync(extractedText);
                return Json(new { success = true, summary = summary });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi xử lý file: " + ex.Message });
            }
        }
    }
}