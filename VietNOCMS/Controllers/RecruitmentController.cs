using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class RecruitmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var model = new RecruitmentCenterViewModel();

           
            model.MarketPosts = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Where(c => c.IsRecruiting && c.IsPublished && c.InstructorId != userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

        
            model.MyCourses = await _context.Courses
                .Where(c => c.InstructorId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

         
            model.PendingApplications = await _context.CourseCollaborators
                .Include(cc => cc.Collaborator)
                .Include(cc => cc.Course)
                .Where(cc => cc.Course.InstructorId == userId && cc.Status == "Pending_Approval")
                .OrderByDescending(cc => cc.InvitedAt)
                .ToListAsync();

          
            var myApplications = await _context.CourseCollaborators
                .Where(cc => cc.CollaboratorId == userId)
                .ToListAsync();

         
            foreach (var app in myApplications)
            {
                if (!model.MyStatus.ContainsKey(app.CourseId))
                {
                    model.MyStatus.Add(app.CourseId, app.Status);
                }
            }
           

            return View(model);
        }

        // API Bật/Tắt trạng thái tuyển dụng (Nút Đăng tin / Gỡ tin)
        [HttpPost]
        public async Task<IActionResult> ToggleRecruitment(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId && c.InstructorId == userId);

            if (course == null) return Json(new { success = false, message = "Không tìm thấy khóa học" });

            // Đảo ngược trạng thái
            course.IsRecruiting = !course.IsRecruiting;
            await _context.SaveChangesAsync();

            string statusMsg = course.IsRecruiting ? "Đã đăng tin tuyển dụng" : "Đã gỡ tin tuyển dụng";
            return Json(new { success = true, message = statusMsg, isRecruiting = course.IsRecruiting });
        }
    }
}