using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietNOCMS.Services;

namespace VietNOCMS.Controllers
{
    [Authorize(Roles = "Instructor")] 
    public class AiInstructorController : Controller
    {
        private readonly GeminiService _geminiService;

        public AiInstructorController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

    
        [HttpPost]
        public async Task<IActionResult> GenerateQuiz(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return Json(new { success = false, message = "Vui lòng nhập chủ đề!" });
            try
            {
                var jsonResult = await _geminiService.GenerateQuizFromTextAsync(topic);
                // Clean JSON nếu AI lỡ thêm markdown
                jsonResult = jsonResult.Replace("```json", "").Replace("```", "").Trim();
                return Json(new { success = true, data = jsonResult });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

    
        [HttpPost]
        public async Task<IActionResult> GenerateSyllabus(string topic, string level)
        {
            try
            {
                var htmlResult = await _geminiService.GenerateSyllabusAsync(topic, level);
                return Json(new { success = true, data = htmlResult });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }

   
        [HttpPost]
        public async Task<IActionResult> AutoGrade(string question, string barem, string answer)
        {
            try
            {
              
                var jsonResult = await _geminiService.AutoGradeAsync(question, barem, answer);

             
                jsonResult = jsonResult.Replace("```json", "").Replace("```", "").Trim();

              
                return Json(new { success = true, data = jsonResult });
            }
            catch (Exception ex) { return Json(new { success = false, message = ex.Message }); }
        }
    }
}