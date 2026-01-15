using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VietNOCMS.Data;
using VietNOCMS.Hubs;
using VietNOCMS.Models;

namespace VietNOCMS.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // 1. Giao diện Chat chính
        public IActionResult Index(int? receiverId)
        {
            // 1. Luôn lấy UserID hiện tại trước
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // 2. Lấy danh sách hội thoại cũ
            // SỬA LỖI: Dùng .ToList() trước rồi mới Cast<dynamic> để tránh lỗi Runtime/Compile với Anonymous Type
            var conversationsData = _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .Select(c => new
                {
                    ConversationId = c.Id,
                    Partner = c.User1Id == userId ? c.User2 : c.User1,
                    LastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault().Content ?? "Hình ảnh/File",
                    LastTime = c.LastMessageAt,
                    UnreadCount = c.Messages.Count(m => m.SenderId != userId && !m.IsRead)
                })
                .ToList(); // Lấy dữ liệu về RAM trước

            // Chuyển sang List<dynamic> để có thể Insert thêm phần tử "giả" (chat mới)
            var conversations = conversationsData.Cast<dynamic>().ToList();

            // 3. Xử lý Logic "Nhắn tin từ nút bấm" (Profile -> Chat)
            if (receiverId.HasValue)
            {
                ViewBag.TargetUserId = receiverId;

                // Kiểm tra xem người này đã có trong danh sách chat chưa?
                // Lưu ý: Cần cast kiểu dynamic để truy cập property, hoặc dùng data gốc để check
                var exists = conversationsData.Any(c => c.Partner.UserId == receiverId.Value);

                if (!exists)
                {
                    // TÌNH HUỐNG: Chưa từng chat bao giờ -> Phải tìm info user này để hiện lên
                    var targetUser = _context.Users.Find(receiverId.Value);

                    if (targetUser != null)
                    {
                        // Tạo một object "hội thoại giả" thêm vào đầu danh sách
                        // Để giao diện có thể hiển thị người này và auto-click vào được
                        conversations.Insert(0, new
                        {
                            ConversationId = 0, // 0 nghĩa là chưa có hội thoại trong DB
                            Partner = targetUser,
                            LastMessage = "Bắt đầu cuộc trò chuyện mới",
                            LastTime = DateTime.Now,
                            UnreadCount = 0
                        });
                    }
                }
            }

            // 4. Trả dữ liệu về View
            ViewBag.Conversations = conversations;
            ViewBag.CurrentUserId = userId;

            return View();
        }

        // 2. API Tìm kiếm người dùng để bắt đầu chat mới
        [HttpGet]
        public async Task<IActionResult> SearchUser(string query)
        {
            if (string.IsNullOrEmpty(query)) return Json(new List<object>());

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var users = await _context.Users
                .Where(u => u.UserId != currentUserId && (u.Email.Contains(query) || u.FullName.Contains(query)))
                .Select(u => new { u.UserId, u.FullName, u.Avatar, u.Email, u.Role })
                .Take(5)
                .ToListAsync();

            return Json(users);
        }

        // 3. API Lấy nội dung tin nhắn của 1 hội thoại
        [HttpGet]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Bảo mật: Kiểm tra xem user có được quyền xem hội thoại này không
            var conversation = await _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c => c.Id == conversationId && (c.User1Id == userId || c.User2Id == userId));

            if (conversation == null) return Forbid();

            // Đánh dấu đã đọc các tin nhắn của đối phương
            var unreadMessages = await _context.ChatMessages
                .Where(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead)
                .ToListAsync();

            if (unreadMessages.Any())
            {
                foreach (var msg in unreadMessages) msg.IsRead = true;
                await _context.SaveChangesAsync();
            }

            var messages = await _context.ChatMessages
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .Select(m => new {
                    m.Id,
                    m.Content,
                    SentAt = m.SentAt.ToString("HH:mm"),
                    IsMine = m.SenderId == userId,
                    SenderId = m.SenderId
                })
                .ToListAsync();

            // Lấy thông tin người chat cùng để hiển thị header
            var partner = conversation.User1Id == userId ? conversation.User2 : conversation.User1;

            return Json(new { success = true, data = messages, partnerName = partner.FullName, partnerAvatar = partner.Avatar });
        }

        // 4. API Gửi tin nhắn
        [HttpPost]
        public async Task<IActionResult> SendMessage(int? conversationId, int? receiverId, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return Json(new { success = false });

            var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Conversation conversation = null;

            // TRƯỜNG HỢP 1: Đã có ConversationId (Chat tiếp)
            if (conversationId.HasValue && conversationId > 0)
            {
                conversation = await _context.Conversations.FindAsync(conversationId.Value);
            }
            // TRƯỜNG HỢP 2: Chat mới từ danh sách tìm kiếm hoặc nút profile (Chưa có ConversationId)
            else if (receiverId.HasValue)
            {
                // Kiểm tra xem đã từng có hội thoại giữa 2 người này chưa
                conversation = await _context.Conversations
                    .FirstOrDefaultAsync(c =>
                        (c.User1Id == senderId && c.User2Id == receiverId) ||
                        (c.User1Id == receiverId && c.User2Id == senderId));

                // Nếu chưa có, tạo mới
                if (conversation == null)
                {
                    conversation = new Conversation
                    {
                        User1Id = senderId,
                        User2Id = receiverId.Value,
                        Type = "Direct",
                        CourseId = null,
                        LastMessageAt = DateTime.Now
                    };
                    _context.Conversations.Add(conversation);
                    await _context.SaveChangesAsync();
                }
            }

            if (conversation == null) return Json(new { success = false, message = "Lỗi hội thoại" });

            // Tạo tin nhắn
            var message = new ChatMessage
            {
                ConversationId = conversation.Id,
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.Now,
                IsRead = false
            };

            _context.ChatMessages.Add(message);

            // Cập nhật thời gian tin nhắn cuối cùng để sort
            conversation.LastMessageAt = DateTime.Now;

            await _context.SaveChangesAsync();

            // Gửi SignalR thông báo cho các bên đang xem hội thoại này
            // Client phải JoinGroup("conv_{id}") mới nhận được
            if (conversation != null)
            {
                await _hubContext.Clients.Group("conv_" + conversation.Id).SendAsync("ReceiveMessage",
                    senderId,                      // Ai gửi
                    message.Content,               // Nội dung
                    message.SentAt.ToString("HH:mm") // Thời gian
                );
            }

            return Json(new { success = true, conversationId = conversation.Id });
        }
    }
}