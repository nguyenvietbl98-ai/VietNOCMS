using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class CollaboratorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollaboratorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // PHẦN 1: GIẢNG VIÊN CHỦ ĐỘNG MỜI (FLOW A)
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Invite(int courseId, string email, string role, string message)
        {
            var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 1. Check quyền: Chỉ chủ khóa học mới được mời
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (course == null || course.InstructorId != senderId)
                return Json(new { success = false, message = "Bạn không có quyền mời người vào khóa học này." });

            // 2. Tìm người được mời
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (receiver == null) return Json(new { success = false, message = "Email không tồn tại trong hệ thống." });
            if (receiver.UserId == senderId) return Json(new { success = false, message = "Không thể tự mời chính mình." });

            // 3. Check xem đã có trong team chưa
            var exists = await _context.CourseCollaborators
                .AnyAsync(c => c.CourseId == courseId && c.CollaboratorId == receiver.UserId && c.Status != "Rejected");
            if (exists) return Json(new { success = false, message = "Người này đã có trong danh sách hoặc đang chờ phản hồi." });

            // 4. Tạo lời mời
            var invite = new CourseCollaborator
            {
                CourseId = courseId,
                CollaboratorId = receiver.UserId,
                Role = role, // "Assistant" hoặc "Substitute"
                CanManageContent = role == "Substitute", // Dạy thay thì được sửa nội dung
                CanGrade = true,
                Status = "Pending_Invite", // <--- Quan trọng: Chờ User đồng ý
                Message = message,
                InvitedAt = DateTime.Now
            };

            _context.CourseCollaborators.Add(invite);

            // 5. Gửi thông báo cho người được mời
            _context.Notifications.Add(new Notification
            {
                UserId = receiver.UserId,
                Title = "Lời mời tham gia giảng dạy",
                Message = $"Giảng viên {User.Identity.Name} đã mời bạn làm {role} cho khóa học '{course.CourseName}'.",
                Type = NotificationType.Info,
                Category = "Recruitment",        // Icon: Cái cặp (bi-briefcase-fill)
                RedirectUrl = "/Collaborator/MyInvitations", // Link để user vào bấm Đồng ý
                CreatedAt = DateTime.Now,
                IsRead = false
            });

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã gửi lời mời thành công!" });
        }

   
        [HttpPost]
        public async Task<IActionResult> Apply(int courseId, string message, string role) // [FIX] Thêm tham số role
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Validate role hợp lệ (Tránh hacker gửi role bậy bạ)
            var validRoles = new[] { "Assistant", "CoInstructor" };
            if (!validRoles.Contains(role))
            {
                return Json(new { success = false, message = "Vị trí ứng tuyển không hợp lệ." });
            }

            // Include Instructor để lấy ID người nhận thông báo
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null) return Json(new { success = false, message = "Khóa học không tồn tại." });

            // 1. Check xem khóa học có đang tuyển không
            if (!course.IsRecruiting)
                return Json(new { success = false, message = "Khóa học này hiện không tuyển dụng." });

            // 2. Check xem đã ứng tuyển chưa
            var exists = await _context.CourseCollaborators
                .AnyAsync(c => c.CourseId == courseId && c.CollaboratorId == userId);
            if (exists) return Json(new { success = false, message = "Bạn đã ứng tuyển hoặc đã ở trong team rồi." });

            // 3. Tạo đơn ứng tuyển với Role được chọn
            var application = new CourseCollaborator
            {
                CourseId = courseId,
                CollaboratorId = userId,
                Role = role, 
                CanManageContent = role == "CoInstructor",
                CanGrade = true, 

                Status = "Pending_Approval",
                Message = message,
                InvitedAt = DateTime.Now
            };

            _context.CourseCollaborators.Add(application);

            var roleDisplay = role == "CoInstructor" ? "Giảng viên phụ" : "Trợ giảng";

            _context.Notifications.Add(new Notification
            {
                UserId = course.InstructorId,
                Title = "Ứng viên mới",
                Message = $"{User.Identity.Name} vừa ứng tuyển vị trí {roleDisplay} cho khóa '{course.CourseName}'.",
                Type = NotificationType.Info,
                Category = "Recruitment",
                RedirectUrl = "/Recruitment?tab=approve",
                CreatedAt = DateTime.Now,
                IsRead = false
            });

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã gửi đơn ứng tuyển! Vui lòng chờ duyệt." });
        }
 
        [HttpGet]
        public async Task<IActionResult> MyInvitations()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var invitations = await _context.CourseCollaborators
                .Include(c => c.Course).ThenInclude(co => co.Instructor)
                .Where(c => c.CollaboratorId == userId && c.Status == "Pending_Invite")
                .OrderByDescending(c => c.InvitedAt)
                .ToListAsync();

            return View(invitations);
        }

        [HttpPost]
        public async Task<IActionResult> RespondToInvite(int id, string action) // action: "Accept" hoặc "Reject"
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var invite = await _context.CourseCollaborators
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id && c.CollaboratorId == userId);

            if (invite == null) return Json(new { success = false, message = "Không tìm thấy lời mời." });

            if (action == "Accept")
            {
                invite.Status = "Accepted";
                invite.ResponseAt = DateTime.Now;

                // Báo lại cho GV
                _context.Notifications.Add(new Notification
                {
                    UserId = invite.Course.InstructorId,
                    Title = "Lời mời được chấp nhận",
                    Message = $"{User.Identity.Name} đã đồng ý tham gia khóa học '{invite.Course.CourseName}'.",
                    Type = NotificationType.Success,
                    Category = "Recruitment",     // Icon: Cái cặp
                    RedirectUrl = "/Instructor/MyCourses",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                });
            }
            else
            {
                invite.Status = "Rejected"; // Hoặc xóa hẳn record: _context.CourseCollaborators.Remove(invite);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // Giảng viên duyệt đơn ứng tuyển của User (Flow B)
        [HttpPost]
        public async Task<IActionResult> ReviewApplication(int id, string action) // action: "Approve" hoặc "Reject"
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Check quyền: Phải là chủ khóa học của đơn ứng tuyển này
            var app = await _context.CourseCollaborators
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.Id == id && c.Course.InstructorId == userId);

            if (app == null) return Json(new { success = false, message = "Không tìm thấy yêu cầu." });

            if (action == "Approve")
            {
                app.Status = "Accepted";
                app.ResponseAt = DateTime.Now;

                // Báo tin vui (Được duyệt)
                _context.Notifications.Add(new Notification
                {
                    UserId = app.CollaboratorId, // Gửi cho người ứng tuyển
                    Title = "Ứng tuyển thành công!",
                    Message = $"Chúc mừng! Bạn đã trở thành trợ giảng cho khóa học '{app.Course.CourseName}'.",

                    // --- CẬP NHẬT QUAN TRỌNG ---
                    Type = NotificationType.Success,        // Màu xanh lá
                    Category = "Recruitment",               // Icon: Cái cặp
                    RedirectUrl = "/Instructor/MyCourses",  // Link dẫn vào trang quản lý để họ bắt đầu làm việc
                                                            // ---------------------------

                    CreatedAt = DateTime.Now,
                    IsRead = false
                });
            }
            else
            {
                app.Status = "Rejected";

                // Báo tin buồn (Từ chối)
                _context.Notifications.Add(new Notification
                {
                    UserId = app.CollaboratorId,
                    Title = "Ứng tuyển thất bại",
                    Message = $"Rất tiếc, yêu cầu tham gia khóa học '{app.Course.CourseName}' của bạn bị từ chối.",

                    // --- CẬP NHẬT QUAN TRỌNG ---
                    Type = NotificationType.Error,          // Màu đỏ
                    Category = "Recruitment",               // Icon: Cái cặp
                    RedirectUrl = "/Recruitment",     // Link quay lại sàn tuyển dụng
                                                            // ---------------------------

                    CreatedAt = DateTime.Now,
                    IsRead = false
                });
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        // GET: /Collaborator/RecruitmentBoard
        [AllowAnonymous] // Cho phép cả khách xem (tùy bạn, hoặc để Authorize nếu bắt buộc đăng nhập)
        public async Task<IActionResult> RecruitmentBoard()
        {
            var jobs = await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Category)
                .Where(c => c.IsRecruiting && c.IsPublished) // Chỉ lấy khóa đang tuyển & đã public
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(jobs);
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