using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietNOCMS.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        // Dùng để định màu sắc (Xanh, Đỏ, Vàng...)
        public NotificationType Type { get; set; } = NotificationType.Info;

        // --- THÊM CỘT NÀY ---
        // Dùng để định Icon (Payment, Assignment, Grade, System, Message...)
        [StringLength(50)]
        public string Category { get; set; } = "System";

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }

    public enum NotificationType
    {
        Info,    // Màu xanh dương
        Warning, // Màu vàng
        Error,   // Màu đỏ
        Success  // Màu xanh lá
    }
}