using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VietNOCMS.Data;
using VietNOCMS.Models;
using VietNOCMS.Services;


namespace VietNOCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AdminController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

      
        public async Task<IActionResult> InstructorRequests()
        {
            var requests = await _context.InstructorRequests
                .Include(r => r.User) 
                .OrderByDescending(r => r.RequestedAt)
                .Select(r => new InstructorRequestViewModel
                {
                    RequestId = r.RequestId,
                    UserId = r.UserId,
                    FullName = r.User.FullName,
                    Email = r.User.Email,
                    Phone = r.User.PhoneNumber,
                    Bio = r.Bio,
                    CvUrl = r.CvUrl,
                    Status = r.Status, 
                    RequestedAt = r.RequestedAt,
                    LinkedIn = r.LinkedInProfile,
                    Facebook = r.FacebookProfile
                })
                .ToListAsync();

            return View(requests);
        }

     
        [HttpGet]
        public async Task<IActionResult> GetRequestDetail(int id)
        {
            var request = await _context.InstructorRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null) return NotFound();

         
            return Json(new
            {
                name = request.User.FullName,
                email = request.User.Email,
                phone = request.User.PhoneNumber ?? "Chưa cập nhật",
              
                bio = string.IsNullOrEmpty(request.Bio) ? "Chưa có mô tả" : request.Bio,
                status = request.Status.ToLower(),
               
                cvUrl = request.CvUrl,
                linkedIn = request.LinkedInProfile,
                facebook = request.FacebookProfile
            });
        }

   
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var request = await _context.InstructorRequests
                .Include(r => r.User) 
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null) return Json(new { success = false, message = "Không tìm thấy yêu cầu" });

         
            request.Status = "Approved";
            request.ReviewedAt = DateTime.Now;

         
            var user = request.User;
            user.Role = "Instructor";

         
            try
            {
                string subject = "Chúc mừng! Hồ sơ giảng viên đã được duyệt - VietN OCMS";
                string content = $@"
                    <h3>Xin chào {user.FullName},</h3>
                    <p>Chúc mừng bạn! Yêu cầu trở thành giảng viên của bạn tại <strong>VietN OCMS</strong> đã được phê duyệt.</p>
                    <p>Bây giờ bạn có thể đăng nhập và bắt đầu tạo khóa học của mình.</p>
                    <br/>
                    <p>Trân trọng,<br/>Đội ngũ VietN OCMS</p>";

                await _emailService.SendEmailAsync(user.Email, subject, content);
            }
            catch (Exception ex)
            {
            
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
            }
        
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã phê duyệt và gửi email thông báo!" });
        }

      
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id, string reason)
        {
           
            var request = await _context.InstructorRequests
                .Include(r => r.User) 
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null) return Json(new { success = false, message = "Không tìm thấy yêu cầu" });

            request.Status = "Rejected";
            request.RejectionReason = reason;
            request.ReviewedAt = DateTime.Now;

         
            try
            {
                string subject = "Thông báo về hồ sơ đăng ký giảng viên - VietN OCMS";
                string content = $@"
                    <h3>Xin chào {request.User.FullName},</h3>
                    <p>Rất tiếc, hồ sơ đăng ký giảng viên của bạn chưa phù hợp tại thời điểm này.</p>
                    <p><strong>Lý do từ chối:</strong> {reason}</p>
                    <p>Bạn có thể cập nhật lại hồ sơ và gửi yêu cầu mới sau.</p>
                    <br/>
                    <p>Trân trọng,<br/>Đội ngũ VietN OCMS</p>";

                await _emailService.SendEmailAsync(request.User.Email, subject, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
            }
         

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đã từ chối và gửi email thông báo." });
        }
    }
}